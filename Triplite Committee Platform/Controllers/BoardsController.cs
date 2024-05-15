using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    [ValidateRole]
    public class BoardsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PreviousBoards()
        {
            return View();
        }
        public IActionResult CurrentBoards()
        {
            return View();
        }
    }
}
