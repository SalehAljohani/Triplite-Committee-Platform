using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _userSign;
        private readonly EmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, UserManager<UserModel> userManager, EmailSender emailSender, SignInManager<UserModel> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
            _userSign = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var userEmail = await _userManager.GetUserAsync(User);
            if (userEmail.EmailConfirmed == false)
            {
                return RedirectToAction("Index", "ConfirmEmail");
            }
            return View();
        }

        public IActionResult Privacy(string name)
        {
            ViewData["name"] = name;
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
