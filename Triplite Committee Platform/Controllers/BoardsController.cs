using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    [ValidateRole]
    public class BoardsController : BaseController
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly AppDbContext _context;
        public BoardsController(UserManager<UserModel> userManager, AppDbContext context)
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
            if(user.EmailConfirmed == false)
            {
                return RedirectToAction("Index", "ConfirmEmail");
            }
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

        [ValidateRole("Head of Department")]
        public async Task<IActionResult> DeptBoards(int? id)
        {
            if(id == null)
            {
                TempData["Error"] = "Scholarship was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var scholarshipDetails = _context.Scholarship.FirstOrDefaultAsync(s => s.Id == id);
            return View(scholarshipDetails);
        }
    }
}
