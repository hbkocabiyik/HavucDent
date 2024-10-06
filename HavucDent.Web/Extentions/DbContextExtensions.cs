using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Identity;
using HavucDent.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HavucDent.Web.Extentions
{
    public static class DbContextExtensions
    {
        public static async Task SeedRolesAndAdminAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var dbContext = scope.ServiceProvider.GetRequiredService<HavucDbContext>();

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

            // Dinamik olarak tüm controller-action'ları reflection ile bulup AccessControl tablosuna ekleme
            var controllerTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract).ToList();

            foreach (var controller in controllerTypes)
            {
                var controllerName = controller.Name.Replace("Controller", "");
                var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(m => m.IsPublic && !m.IsDefined(typeof(NonActionAttribute))).ToList();

                foreach (var action in actions)
                {
                    var actionName = action.Name;

                    // Admin için AccessControl'de bu controller-action var mı kontrol et
                    if (!await dbContext.AccessControls.AnyAsync(ac =>
                            ac.RoleName == "Admin" &&
                            ac.ControllerName == controllerName &&
                            ac.ActionName == actionName))
                    {
                        // Yoksa ekle
                        dbContext.AccessControls.Add(new AccessControl
                        {
                            RoleName = "Admin",
                            ControllerName = controllerName,
                            ActionName = actionName
                        });
                    }
                }
            }

            await dbContext.SaveChangesAsync();

        }
    }
}
