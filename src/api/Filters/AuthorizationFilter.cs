using System.Linq;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace api.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private const string _authSchema = "Bearer ";
        private readonly ITokenProviderService _tokenProviderService;
        private readonly ILogger _logger;

        public AuthorizationFilter(ITokenProviderService tokenProviderService, ILoggerFactory loggerFactory)
        {
            _tokenProviderService = tokenProviderService;
            _logger = loggerFactory.CreateLogger<AuthorizationFilter>();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var cad = context.ActionDescriptor as ControllerActionDescriptor;
            if (cad != null)
            {
                var attrs = cad.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);
                if (attrs != null && attrs.Any())
                    return;
            }

            var userId = ValidateRequest(context.HttpContext.Request);
            if (!userId.HasValue)
                context.Result = new UnauthorizedResult();
            context.HttpContext.Items.Add("user", userId);
        }

        private long? ValidateRequest(HttpRequest request)
        {
            if (request.Headers.TryGetValue("Authorization", out StringValues values))
            {
                var authHeader = values.FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith(_authSchema))
                {
                    var authToken = authHeader.Substring(_authSchema.Length);
                    return _tokenProviderService.ValidateToken(authToken);
                }
            }
            _logger.LogDebug("Request does not have the Authorization header");
            return null;
        }
    }
}
