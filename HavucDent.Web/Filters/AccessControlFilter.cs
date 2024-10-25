using HavucDent.Domain.Entities;
using HavucDent.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using HavucDent.Infrastructure.Identity;

namespace HavucDent.Web.Filters
{
    public class AccessControlFilter : IAsyncActionFilter
    {
        private readonly HavucDbContext _context;
        private readonly UserManager<AppUser> _userManager; // AppUser kullanımı

        public AccessControlFilter(HavucDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                var currentUser = await _userManager.GetUserAsync(user);
                var userRoles = await _userManager.GetRolesAsync(currentUser);

                var controllerName = context.ActionDescriptor.RouteValues["controller"];
                var actionName = context.ActionDescriptor.RouteValues["action"];

                // Veritabanında rol ile controller-action eşleştirmesini kontrol et
                var hasAccess = _context.AccessControls.Any(ac =>
                    userRoles.Contains(ac.RoleName) &&
                    ac.ControllerName == controllerName &&
                    ac.ActionName == actionName);

                if (!hasAccess)
                {
                    context.Result = new ForbidResult(); // Erişim yoksa "403 Forbidden"
                    return;
                }
            }

            await next(); // Erişim varsa devam et
			}
    }
}
