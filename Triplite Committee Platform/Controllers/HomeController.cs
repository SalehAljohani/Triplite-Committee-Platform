using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Triplite_Committee_Platform.Services;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Azure;
using Microsoft.AspNetCore.Authorization;

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
