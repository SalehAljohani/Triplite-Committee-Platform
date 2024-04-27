using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.ViewModels;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;

        public SearchController(AppDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (user.EmailConfirmed == false)
            {
                return RedirectToAction("Index", "ConfirmEmail");
            }

            var roleVerify = await _userManager.GetRolesAsync(user);
            if (roleVerify != null && roleVerify.Count > 1)
            {
                return RedirectToAction("ChooseRole", "ChooseRole");
            }

            if (User.IsInRole("Admin"))
            {
                var college = await _context.College.ToListAsync();
                var department = await _context.Department.ToListAsync();
                ViewData["Colleges"] = new SelectList(college, "CollegeNo", "CollegeName");
                ViewData["Departments"] = new SelectList(department, "DeptNo", "DeptName");
            }
            if (User.IsInRole("Vice Dean") || User.IsInRole("Dean"))
            {
                var college = await _context.College.Where(c => c.CollegeNo == user.Department.College.CollegeNo).ToListAsync();
                ViewData["Departments"] = new SelectList(college, "DeptNo", "DeptName");
            }
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string? search, int? SelectedCollege, int? SelectedDepartment)
        {
            if (string.IsNullOrWhiteSpace(search)) // Check if search is null or empty (search = "") or whitespace ( )
            {
                TempData["Message"] = "No search term was entered. Please enter a valid identifier such as National ID, Board number, Phone number, Employee ID, or Scholarship name.";
                return RedirectToAction(nameof(Index));
            }

            var viewModelList = new List<SearchViewModel>();
            var results = await _context.Scholarship
                .Include(s => s.Board)
                .Include(s => s.Department)
                    .ThenInclude(d => d.College)
                .Where(s => (s.National_ID == search || s.Name.ToLower() == search.ToLower() || s.Phone == search || s.EmployeeID == search) &&
                            (SelectedCollege == null || s.Department.College.CollegeNo == SelectedCollege) &&
                            (SelectedDepartment == null || s.Department.DeptNo == SelectedDepartment))
                .ToListAsync();

            if (results.Any())
            {
                foreach (var item in results)
                {
                    var viewModel = new SearchViewModel
                    {
                        CollegeName = item.Department?.College?.CollegeName,
                        DeptName = item.Department?.DeptName,
                        Boards = item.Board,
                        Name = item.Name,
                    };
                    viewModelList.Add(viewModel);
                }
                return View(viewModelList);
            }
            else
            {
                TempData["Message"] = "No results found.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}