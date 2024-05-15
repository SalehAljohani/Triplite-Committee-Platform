using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateRole("Admin")]
    public class DepartmentController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly IStringLocalizer<DepartmentController> Localizer;

        public DepartmentController(AppDbContext context, UserManager<UserModel> userManager, IStringLocalizer<DepartmentController>localizer)
        {
            _context = context;
            _userManager = userManager;
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
                return RedirectToAction("Index", "ConfirmEmail");
            }
            return RedirectToAction("Index", "College");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                string deptNotFound = @Localizer["deptNotFound"];
                TempData["Error"] = deptNotFound;
                return RedirectToAction("Index", "College");
            }

            var departmentModel = await _context.Department
                .Include(d => d.College)
                .FirstOrDefaultAsync(m => m.DeptNo == id);
            if (departmentModel == null)
            {
                string deptNotFound = @Localizer["deptNotFound"];
                TempData["Error"] = deptNotFound;
                return RedirectToAction("Index", "College");
            }

            return View(departmentModel);
        }

        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                string collegeNotFound = @Localizer["collegeNotFound"];
                TempData["Error"] = collegeNotFound;
                return RedirectToAction("Index", "College");
            }
            if(_context.College.Find(id) == null)
            {
                string collegeNotFound = @Localizer["collegeNotFound"];
                TempData["Error"] = collegeNotFound;
                return RedirectToAction("Index", "College");
            }
            ViewData["CollegeNo"] = new SelectList(_context.College, "CollegeNo", "CollegeName", id);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CollegeNo,DeptName")] DepartmentModel departmentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departmentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CollegeNo"] = new SelectList(_context.College, "CollegeNo", "CollegeName", departmentModel.CollegeNo);
            return View(departmentModel);
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                string deptNotFound = @Localizer["deptNotFound"];
                TempData["Error"] = deptNotFound;
                return RedirectToAction("Index", "College");
            }

            var departmentModel = await _context.Department.FindAsync(id);
            if (departmentModel == null)
            {
                string deptNotFound = @Localizer["deptNotFound"];
                TempData["Error"] = deptNotFound;
                return RedirectToAction("Index", "College");
            }
            ViewData["CollegeNo"] = new SelectList(_context.College, "CollegeNo", "CollegeName", departmentModel.CollegeNo);
            return View(departmentModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CollegeNo,DeptName,DeptNo")] DepartmentModel departmentModel)
        {
            if (id != departmentModel.DeptNo)
            {
                string deptNotFound = @Localizer["deptNotFound"];
                TempData["Error"] = deptNotFound;
                return RedirectToAction("Index", "College");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departmentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentModelExists(departmentModel.DeptNo))
                    {
                        string deptNotFound = @Localizer["deptNotFound"];
                        TempData["Error"] = deptNotFound;
                        return RedirectToAction("Index", "College");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CollegeNo"] = new SelectList(_context.College, "CollegeNo", "CollegeName", departmentModel.CollegeNo);
            return View(departmentModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                string deptNotFound = @Localizer["deptNotFound"];
                TempData["Error"] = deptNotFound;
                return RedirectToAction("Index", "College");
            }

            if (_context.Department.Find(id) == null)
            {
                string deptNotFound = @Localizer["deptNotFound"];
                TempData["Error"] = deptNotFound;
                return RedirectToAction("Index", "College");
            }

            var departmentModel = await _context.Department
                .Include(d => d.College)
                .FirstOrDefaultAsync(m => m.DeptNo == id);
            if (departmentModel == null)
            {
                string deptNotFound = @Localizer["deptNotFound"];
                TempData["Error"] = deptNotFound;
                return RedirectToAction("Index", "College");
            }

            var hasUsers = await _context.User.AnyAsync(u => u.DeptNo == id);

            if (hasUsers)
            {
                string deptUsers = @Localizer["deptUsers"];
                ViewData["UserMessage"] = deptUsers;
                
                return View(departmentModel);
            }

            return View(departmentModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departmentModel = await _context.Department.FindAsync(id);
            var hasUsers = await _context.User.AnyAsync(u => u.DeptNo == id);

            if (hasUsers)
            {
                string deptUsers = @Localizer["deptUsers"];
                TempData["UserMessage"] = deptUsers;

                return View(departmentModel);
            }
            if (departmentModel != null)
            {
                _context.Department.Remove(departmentModel);
                string deptDeleted = @Localizer["deptDeleted"];
                ViewData["Message"] = deptDeleted;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentModelExists(int id)
        {
            return _context.Department.Any(e => e.DeptNo == id);
        }
    }
}
