using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Triplite_Committee_Platform.Services
{
    public class ValidateRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _requiredRoles;
        public ValidateRoleAttribute(params string[] requiredRoles)
        {
            _requiredRoles = requiredRoles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var activeRole = httpContext.Session.GetString("ActiveRole");

            if (string.IsNullOrEmpty(activeRole))
            {
                var roleClaims = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();

                if (roleClaims.Count == 1)
                {
                    // If there's exactly one role claim, set it as the active role and proceed
                    httpContext.Session.SetString("ActiveRole", roleClaims.First().Value);
                    activeRole = roleClaims.First().Value;
                }
                else if (roleClaims.Count > 1)
                {
                    // More than one role, prompt user to choose
                    context.Result = new RedirectToActionResult("Index", "ChooseRole", null);
                    return;
                }
            }

            if (_requiredRoles != null && _requiredRoles.Length > 0 && !_requiredRoles.Contains(activeRole))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
