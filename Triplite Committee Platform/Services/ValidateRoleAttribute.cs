using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
                if (httpContext.User.Claims.Count() <= 1) 
                {
                    return; 
                }
                context.Result = new RedirectToActionResult("Index", "ChooseRole", null);
                return;
            }

            if (_requiredRoles != null && _requiredRoles.Length > 0 && !_requiredRoles.Contains(activeRole))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
