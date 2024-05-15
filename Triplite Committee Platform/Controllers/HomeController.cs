using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Diagnostics;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    [ValidateRole]
    public class HomeController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly IStringLocalizer<HomeController> Localizer;

        public HomeController(UserManager<UserModel> userManager, AppDbContext context, IStringLocalizer<HomeController>localizer)
        {
            _userManager = userManager;
            _context = context;
            Localizer = localizer;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (user.EmailConfirmed == false)
            {
                string confirmEmail = @Localizer["confirmEmail"];
                TempData["Message"] = confirmEmail;
                return RedirectToAction("Index", "ConfirmEmail");
            }

            var annoncements = await _context.Announcements.Where(a => a.DeptNo == user.DeptNo).OrderByDescending(x => x.Id).ToListAsync();

            ViewData["Announce"] = annoncements;

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
