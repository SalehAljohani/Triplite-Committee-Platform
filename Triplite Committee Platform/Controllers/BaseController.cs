using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Triplite_Committee_Platform.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var activeRole = HttpContext.Session.GetString("ActiveRole");

            ViewBag.ActiveRole = activeRole;
        }

    }
}
