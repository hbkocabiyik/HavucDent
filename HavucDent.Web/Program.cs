using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Persistence;
using HavucDent.Web.Extentions;
using HavucDent.Web.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AccessControlFilter>(); // Dinamik yetkilendirme filtresi eklendi
});


// DbContext konfig�rasyonu
builder.Services.AddDbContext<HavucDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Identity konfig�rasyonu
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<HavucDbContext>()
    .AddDefaultTokenProviders();

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

app.UseAuthentication(); // Kimlik do�rulama i�lemleri
app.UseAuthorization();

// Veritaban� Seed i�lemi (Roller ve Y�netici kullan�c�s�n� ekleme)
await app.SeedRolesAndAdminAsync();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
