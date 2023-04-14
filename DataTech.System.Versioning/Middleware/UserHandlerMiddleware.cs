
using DataTech.System.Versioning.Helpers.Identity;
using DataTech.System.Versioning.Models.Common; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DataTech.System.Versioning.Middleware
{
    public class UserHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserHandlerMiddleware> _logger;
        private readonly IMemoryCache _cache;

        public UserHandlerMiddleware(RequestDelegate next, ILogger<UserHandlerMiddleware> logger, IMemoryCache cache)
        {
            _next = next;
            _logger = logger;
            _cache = cache;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var userIdentity = UserIdentityHelper.GetUserFromIdentity(httpContext);
                httpContext.Items[SystemVariables.CurrentUser] = userIdentity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while handlling user");
            }
            await _next(httpContext);
        }
    }

    public static class UserHanlerMiddlewareExtension
    {
        public static IApplicationBuilder UseUserHandler(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<UserHandlerMiddleware>();
        }
    }

}
