using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Identity;
using HavucDent.Infrastructure.Persistence;
using HavucDent.Web.Extentions;
using HavucDent.Web.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext konfigürasyonu
builder.Services.AddDbContext<HavucDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Identity konfigürasyonu
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<HavucDbContext>()
    .AddDefaultTokenProviders();


// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AccessControlFilter>(); // Dinamik yetkilendirme filtresi eklendi
});

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

app.UseAuthentication(); // Kimlik doðrulama iþlemleri
app.UseAuthorization();

// Veritabaný Seed iþlemi (Roller ve Yönetici kullanýcýsýný ekleme)
await app.SeedRolesAndAdminAsync();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");



app.Run();
