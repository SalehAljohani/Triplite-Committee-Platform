using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Triplite_Committee_Platform.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var httpContext = context.HttpContext;

            var activeRole = HttpContext.Session.GetString("ActiveRole");
            ViewBag.ActiveRole = activeRole;

            var userRoles = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role);
            ViewBag.DisplayRoleChange = true;
            if (userRoles.Count() == 1)
            {
                ViewBag.DisplayRoleChange = false;
            }
        }
    }
}
