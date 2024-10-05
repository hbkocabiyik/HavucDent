using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace HavucDent.Web.Extentions
{
    public static class DbContextExtensions
    {
        public static async Task SeedRolesAndAdminAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roles = new string[] { "Admin", "Doctor", "Assistant" };

            // Roller yoksa oluşturuluyor
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // İlk yönetici kullanıcısı oluşturulacak
            var adminEmail = "hbkocabiyik@gmail.com";
            var adminPassword = "Admin123!"; // Şifre şifrelenerek saklanacak

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Hüseyin Burak",
                    LastName = "Kocabıyık",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
