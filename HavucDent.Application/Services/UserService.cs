using HavucDent.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HavucDent.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetLoggedUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId != null ? int.Parse(userId) : 0;
        }
    }
}