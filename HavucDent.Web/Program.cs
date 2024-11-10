using HavucDent.Application.Interfaces;
using HavucDent.Application.Mappings;
using HavucDent.Application.Services;
using HavucDent.Common.Logging;
using HavucDent.Common.Services;
using HavucDent.Common.Settings;
using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Identity;
using HavucDent.Infrastructure.Interfaces;
using HavucDent.Infrastructure.Persistence;
using HavucDent.Infrastructure.Repositories;
using HavucDent.Infrastructure.UnitOfWork;
using HavucDent.Web.Extentions;
using HavucDent.Web.Filters;
using HavucDent.Web.Hubs;
using HavucDent.Web.Logging;
using HavucDent.Web.Middleware;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using System.Net;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

#region Identity Keys

var dataProtectionPath = Path.Combine(Directory.GetCurrentDirectory(), "DataProtectionKeys");

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionPath))
    .SetApplicationName("HavucDent");

#endregion


#region Kestrel

var configuration = builder.Configuration;
var domain = configuration["ApplicationSettings:Domain"] ?? "http://localhost";
var httpPort = configuration["ApplicationSettings:Port"] ?? "1903";
var httpsPort = configuration["ApplicationSettings:HttpsPort"] ?? "443";
var thumbprint = configuration["ApplicationSettings:SslThumbprint"];

// Kestrel yap�land�rmas�
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    // HTTP i�in dinleme
    serverOptions.Listen(IPAddress.Any, int.Parse(httpPort));

    // HTTPS i�in dinleme
    if (!string.IsNullOrEmpty(thumbprint))
    {
        // Sertifikay� thumbprint ile bul
        var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.ReadOnly);

        var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
        if (certs.Count > 0)
        {
            var certificate = certs[0];
            serverOptions.Listen(IPAddress.Any, int.Parse(httpsPort), listenOptions =>
            {
                listenOptions.UseHttps(certificate);
            });
        }
        store.Close();
    }
});

#endregion

// DbContext konfig�rasyonu
builder.Services.AddDbContext<HavucDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//AutoMapper DI ekleme
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(typeof(ApplicationMappingProfile));


// Identity konfig�rasyonu
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true; // En az bir rakam
        options.Password.RequiredLength = 8;  // Minimum 8 karakter
        options.Password.RequireNonAlphanumeric = true; // �zel karakter gereksinimi
        options.Password.RequireUppercase = true; // En az bir b�y�k harf
        options.Password.RequireLowercase = true; // En az bir k���k harf
    })
    .AddEntityFrameworkStores<HavucDbContext>()
    .AddDefaultTokenProviders();

// NLog yap�land�rmas�n� y�kleme
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Logging servisini DI ekleme
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(30); 
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddHttpContextAccessor();

#region SignalR

builder.Services.AddSignalR();

#endregion

#region Services

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ILaboratoryService, LaboratoryService>();
builder.Services.AddScoped<IPersonelService, PersonelService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddSingleton<IEmailSender, EmailSender>();

#endregion

#region Repositories

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // UnitOfWork registration
builder.Services.AddScoped<IRepository<Appointment>, Repository<Appointment>>(); // Appointment repository
builder.Services.AddScoped<IRepository<Product>, Repository<Product>>(); // Product repository
builder.Services.AddScoped<IRepository<Laboratory>, Repository<Laboratory>>(); // Laboratory repository

#endregion


// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter()); // T�m controller'lara yetkilendirme zorunlulu�u
    options.Filters.Add<AccessControlFilter>(); // Dinamik yetkilendirme filtresi eklendi
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<RequestLoggingMiddleware>(); // Loglama Middleware

app.UseAuthentication(); // Kimlik do�rulama i�lemleri
app.UseAuthorization();

// Veritaban� Seed i�lemi (Roller ve Y�netici kullan�c�s�n� ekleme)
await app.SeedRolesAndAdminAsync();

// Access Denied ve Unauthorized y�nlendirme
app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 403)
    {
        context.HttpContext.Response.Redirect("/Account/AccessDenied");
    }
    else if (context.HttpContext.Response.StatusCode == 401)
    {
        context.HttpContext.Response.Redirect("/Account/Login");
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapHub<AppointmentHub>("/appointmentHub");

app.Run();
