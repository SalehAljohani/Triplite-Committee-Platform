using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Triplite_Committee_Platform.Services
{
    public class ValidateRoleAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var activeRole = httpContext.Session.GetString("ActiveRole");

            if(!httpContext.User.Identity.IsAuthenticated || string.IsNullOrEmpty(activeRole) || httpContext.User.IsInRole(activeRole))
            {
                context.Result = new RedirectToActionResult("ChooseRole", "ChooseRole", null);
            }    
        }
    }
}
