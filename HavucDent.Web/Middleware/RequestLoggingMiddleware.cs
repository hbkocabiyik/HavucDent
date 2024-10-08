using HavucDent.Common.Logging;
using System.Security.Claims;

namespace HavucDent.Web.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // İstek bilgilerini al
            var user = context.User.Identity.IsAuthenticated
                ? context.User.FindFirst(ClaimTypes.Name)?.Value
                : "Anonymous";

            var path = context.Request.Path;
            _logger.LogInfo($"User {user} is accessing {path}");

            try
            {
                // Bir sonraki middleware'i çağır
                await _next(context);
            }
            catch (Exception ex)
            {
                // Hata durumunu logla
                _logger.LogError($"Something went wrong: {ex.Message}");
                throw; // Hatanın yayılmasına izin ver
            }

            // Başarılı isteği logla
            _logger.LogInfo($"User {user} successfully accessed {path}");
        }
    }
}