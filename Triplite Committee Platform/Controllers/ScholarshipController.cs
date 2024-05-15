using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    [ValidateRole]
    public class ScholarshipController : BaseController
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly AppDbContext _context;

        public ScholarshipController(UserManager<UserModel> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
                return RedirectToAction("Index", "ConfirmEmail");
            }
            return View();
        }
        public IActionResult RegisterScholarship()
        {
            return View();
        }
        public async Task<IActionResult> NewScholarshipRequest()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var newRequest = _context.Scholarship.Where(r => r.Status == "Request Scholarship" && r.DeptNo == user.DeptNo).ToList();

            return View(newRequest);
        }
    }
}
