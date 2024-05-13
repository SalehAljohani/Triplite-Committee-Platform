using Microsoft.AspNetCore.Mvc;

namespace Triplite_Committee_Platform.Controllers
{
    public class BoardsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
