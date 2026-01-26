using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;

namespace InstitutionPanel.Filters
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string? AllowedUserTypes { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(m => m is AllowAnonymousAttribute);

            if (hasAllowAnonymous)
                return;

            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new RedirectToActionResult(
                    "AccessDenied",
                    "Error",
                    null
                );
                return;
            }

            if (!string.IsNullOrWhiteSpace(AllowedUserTypes))
            {
                var userType = user.Claims
                    .FirstOrDefault(c => c.Type == "UserType")?.Value;

                if (string.IsNullOrEmpty(userType))
                {
                    context.Result = new RedirectToActionResult(
                        "AccessDenied",
                        "Error",
                        null
                    );
                    return;
                }

                var allowedTypes = AllowedUserTypes
                    .Split(',')
                    .Select(t => t.Trim().ToLower());

                if (!allowedTypes.Contains(userType.ToLower()))
                {
                    context.Result = new RedirectToActionResult(
                        "AccessDenied",
                        "Error",
                        null
                    );
                }
            }
        }
    }
}
