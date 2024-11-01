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
using HavucDent.Web.Logging;
using HavucDent.Web.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// DbContext konfigürasyonu
builder.Services.AddDbContext<HavucDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//AutoMapper DI ekleme
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(typeof(ApplicationMappingProfile));


// Identity konfigürasyonu
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<HavucDbContext>()
    .AddDefaultTokenProviders();

// NLog yapýlandýrmasýný yükleme
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Logging servisini DI ekleme
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

#region Services

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ILaboratoryService, LaboratoryService>();
builder.Services.AddScoped<IPersonelService, PersonelService>();
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
    options.Filters.Add(new AuthorizeFilter()); // Tüm controller'lara yetkilendirme zorunluluðu
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

app.UseAuthentication(); // Kimlik doðrulama iþlemleri
app.UseAuthorization();

// Veritabaný Seed iþlemi (Roller ve Yönetici kullanýcýsýný ekleme)
await app.SeedRolesAndAdminAsync();

// Access Denied ve Unauthorized yönlendirme
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



app.Run();
