using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Reflection.Metadata;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;
using Triplite_Committee_Platform.ViewModels;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    public class SearchController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly IStringLocalizer<SearchController> Localizer;

        public SearchController(AppDbContext context, UserManager<UserModel> userManager, IStringLocalizer<SearchController>localizer)
        {
            _context = context;
            _userManager = userManager;
            Localizer = localizer;
        }
        [ValidateRole]
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

            var activeRole = HttpContext.Session.GetString("ActiveRole");


            if (activeRole == "Admin")
            {
                var college = await _context.College.ToListAsync();
                var department = await _context.Department.ToListAsync();

                ViewData["Colleges"] = new SelectList(college, "CollegeNo", "CollegeName");
                var departmentsData = department.Select(d => new
                {
                    DeptNo = d.DeptNo,
                    DeptName = d.DeptName,
                    CollegeNo = d.CollegeNo
                }).ToList();

                ViewData["Departments"] = Newtonsoft.Json.JsonConvert.SerializeObject(departmentsData);
            }
            else if (activeRole == "College Dean" || activeRole == "Vice Dean")
            {
                var userDept = await _context.Department.Where(c => c.DeptNo == user.DeptNo).FirstOrDefaultAsync();
                if (userDept == null)
                {
                    string unableLoad = @Localizer["unableLoad"];
                    TempData["Message"] = unableLoad;
                    return RedirectToAction(nameof(Index));
                }
                var department = await _context.Department.Where(d => d.CollegeNo == userDept.CollegeNo).ToListAsync();
                ViewData["Departments"] = new SelectList(department, "DeptNo", "DeptName");

            }
            else if (activeRole == "Head of Department" || activeRole == "Department Member")
            {
                ViewData["Colleges"] = null;
                ViewData["Departments"] = null;
            }
            return View();
        }


        [ValidateRole]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string? search, int? SelectedCollege, int? SelectedDepartment)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                string noTerm = @Localizer["noTerm"];
                TempData["Message"] = noTerm;
                return RedirectToAction(nameof(Index));
            }

            var activeRole = HttpContext.Session.GetString("ActiveRole");
            if (activeRole == "Admin")
            {
                var viewModelList = new List<SearchViewModel>();
                var results = await _context.Scholarship
                    .Include(s => s.Board)
                        .ThenInclude(b => b.RequestType!)
                    .Include(s => s.Department)
                        .ThenInclude(d => d.College!)
                    .Where(s => (s.National_ID == search || s.Name.ToLower() == search.ToLower() || s.Phone == search || s.EmployeeID == search) &&
                                (SelectedCollege == null || s.Department.College!.CollegeNo == SelectedCollege) &&
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
                    var college = await _context.College.ToListAsync();
                    var department = await _context.Department.ToListAsync();

                    ViewData["Colleges"] = new SelectList(college, "CollegeNo", "CollegeName");
                    var departmentsData = department.Select(d => new
                    {
                        DeptNo = d.DeptNo,
                        DeptName = d.DeptName,
                        CollegeNo = d.CollegeNo
                    }).ToList();

                    ViewData["Departments"] = Newtonsoft.Json.JsonConvert.SerializeObject(departmentsData);
                    return View(viewModelList);
                }
                else
                {
                    string noResult = @Localizer["noResult"];
                    TempData["Message"] = noResult;
                    return RedirectToAction(nameof(Index));
                }
            }
            else if (activeRole == "College Dean" || activeRole == "Vice Dean")
            {
                var viewModelList = new List<SearchViewModel>();
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                var userDept = await _context.Department.Where(c => c.DeptNo == user.DeptNo).FirstOrDefaultAsync();
                var department = await _context.Department.Where(d => d.CollegeNo == userDept.CollegeNo).ToListAsync();

                var results = await _context.Scholarship
                    .Include(s => s.Board)
                        .ThenInclude(b => b.RequestType)
                    .Include(s => s.Department)
                        .ThenInclude(d => d.College)
                    .Where(s => (s.National_ID == search || s.Name.ToLower() == search.ToLower() || s.Phone == search || s.EmployeeID == search) &&
                                (SelectedDepartment == null || s.Department.DeptNo == SelectedDepartment) &&
                                 department.Contains(s.Department))
                    .ToListAsync();

                if (results.Any())
                {
                    foreach (var item in results)
                    {
                        var viewModel = new SearchViewModel
                        {
                            DeptName = item.Department?.DeptName,
                            Boards = item.Board,
                            Name = item.Name,
                        };
                        viewModelList.Add(viewModel);
                    }
                    if (userDept == null)
                    {
                        TempData["Message"] = "Unable to load user department.";
                        return RedirectToAction(nameof(Index));
                    }
                    ViewData["Departments"] = new SelectList(department, "DeptNo", "DeptName");

                    return View(viewModelList);
                }
                else
                {
                    string noResult = @Localizer["noResult"];
                    TempData["Message"] = noResult;
                    return RedirectToAction(nameof(Index));
                }
            }
            else if (activeRole == "Head of Department" || activeRole == "Department Member")
            {
                var user = await _userManager.GetUserAsync(User);
                var viewModelList = new List<SearchViewModel>();
                var results = await _context.Scholarship
                    .Include(s => s.Board)
                        .ThenInclude(b => b.RequestType)
                    .Include(s => s.Department)
                        .ThenInclude(d => d.College)
                    .Where(s => (s.National_ID == search || s.Name.ToLower() == search.ToLower() || s.Phone == search || s.EmployeeID == search) &&
                          (s.DeptNo == user.DeptNo))
                    .ToListAsync();

                if (results.Any())
                {
                    foreach (var item in results)
                    {
                        var viewModel = new SearchViewModel
                        {
                            Boards = item.Board,
                            Name = item.Name,
                        };
                        viewModelList.Add(viewModel);
                    }
                    ViewData["Colleges"] = null;
                    ViewData["Departments"] = null;
                    return View(viewModelList);
                }
                else
                {
                    string noResult = @Localizer["noResult"];
                    TempData["Message"] = noResult;
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                string notAuth = @Localizer["notAuth"];
                TempData["Message"] = notAuth;
                return RedirectToAction(nameof(Index));
            }
        }
        
    }
}