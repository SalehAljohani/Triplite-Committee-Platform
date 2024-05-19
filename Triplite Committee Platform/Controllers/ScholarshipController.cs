using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        [ValidateRole("Vice Dean")]
        public async Task<IActionResult> RegisterScholarship()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var userDept = await _context.Department.Where(d => d.DeptNo == user.DeptNo).FirstOrDefaultAsync();
            if (userDept == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var college = await _context.College.Where(c => c.CollegeNo == userDept.DeptNo).FirstOrDefaultAsync();
            if(college == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var deptList = await _context.Department.Where(d => d.CollegeNo == college.CollegeNo).ToListAsync();
            var request = await _context.RequestType.ToListAsync();
            if(request == null || deptList == null)
            {
                TempData["Error"] = "Error occured while fetching data.";
                return RedirectToAction("RegisterScholarship");
            }
            ViewData["Request"] = new SelectList(request, "RequestTypeID", "RequestTypeName");
            ViewData["Departments"] = new SelectList(deptList, "DeptNo", "DeptName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateRole("Vice Dean")]
        public async Task<IActionResult> RegisterScholarship(ScholarshipModel model, int? SelectedDepartment)
        {
            if (SelectedDepartment != null)
            {
                model.DeptNo = SelectedDepartment.Value;
            }
            else
            {
                TempData["Error"] = "Department is required.";
                return RedirectToAction("RegisterScholarship");
            }
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields.";
                return RedirectToAction("RegisterScholarship");
            }
            model.Status = model.Status.ToLower();
            await _context.Scholarship.AddAsync(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Scholarship request redirected to Department.";
            ModelState.Clear();
            return RedirectToAction("RegisterScholarship");
        }

        [ValidateRole("Head of Department", "Department Member")]
        public async Task<IActionResult> NewScholarshipRequest()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var newRequest = await _context.Scholarship
                .Where(r => r.DeptNo == user.DeptNo)
                .Where(r => r.Board == null || r.Board.Count == 0)
                .ToListAsync();
            return View(newRequest);
        }

    }
}
