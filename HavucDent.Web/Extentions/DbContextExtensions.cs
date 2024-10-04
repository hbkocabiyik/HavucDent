using HavucDent.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HavucDent.Web.Extentions
{
    public static class DbContextExtensions
    {
        public static async Task SeedRolesAndAdminAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

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
            var adminEmail = "admin@admin.com";
            var adminPassword = "Admin123!"; // Şifre şifrelenerek saklanacak

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
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
