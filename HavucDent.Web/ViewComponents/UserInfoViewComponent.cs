using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HavucDent.Infrastructure.Identity;

public class UserInfoViewComponent : ViewComponent
{
	private readonly UserManager<AppUser> _userManager;

	public UserInfoViewComponent(UserManager<AppUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task<IViewComponentResult> InvokeAsync()
	{
		var userName = "Misafir";

		if (User.Identity.IsAuthenticated)
		{
			var currentUser = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
			if (currentUser != null)
			{
				userName = $"{currentUser.FirstName} {currentUser.LastName}";
			}
		}

		return View("Default", userName);
	}
}