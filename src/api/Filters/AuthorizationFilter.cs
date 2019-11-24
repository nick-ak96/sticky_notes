using System.Linq;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace api.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private const string _authSchema = "Bearer ";
		private readonly ITokenProviderService _tokenProviderService;

		public AuthorizationFilter(ITokenProviderService tokenProviderService)
		{
			_tokenProviderService = tokenProviderService;
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

			return;
            if (!ValidateRequest(context.HttpContext.Request))
                context.Result = new UnauthorizedResult();
        }

        private bool ValidateRequest(HttpRequest request)
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
            return false;
        }
    }
}
