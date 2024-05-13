using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateRole("Admin")]
    public class CollegeController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;

        public CollegeController(AppDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            var college = await _context.College.Include(c => c.Department).ToListAsync();
            return View(college);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "College not found.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["Departments"] = _context.Department.Where(d => d.CollegeNo == id).ToList();

            var collegeModel = await _context.College
                .FirstOrDefaultAsync(m => m.CollegeNo == id);
            if (collegeModel == null)
            {
                TempData["Error"] = "College not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(collegeModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CollegeName")] CollegeModel collegeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(collegeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(collegeModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "College not found.";
                return RedirectToAction(nameof(Index));
            }

            var collegeModel = await _context.College.FindAsync(id);
            if (collegeModel == null)
            {
                TempData["Error"] = "College not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(collegeModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CollegeNo,CollegeName")] CollegeModel collegeModel)
        {
            if (id != collegeModel.CollegeNo)
            {
                TempData["Error"] = "College not found.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(collegeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollegeModelExists(collegeModel.CollegeNo))
                    {
                        TempData["Error"] = "College not found.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(collegeModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "College not found.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["Departments"] = _context.Department.Where(d => d.CollegeNo == id).ToList();
            var collegeModel = await _context.College.FirstOrDefaultAsync(m => m.CollegeNo == id);
            if (collegeModel == null)
            {
                TempData["Error"] = "College not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(collegeModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collegeModel = await _context.College.FindAsync(id);
            var existingDepartments = _context.Department.Where(d => d.CollegeNo == id).ToList();
            if(existingDepartments.Count() > 0)
            {
                TempData["DeleteMessage1"] = "Cannot delete college with existing departments.";
                TempData["DeleteMessage2"] = "Please Delete the departments First";
                return RedirectToAction("Delete");
            }
            if (collegeModel != null)
            {
                _context.College.Remove(collegeModel);
                TempData["Message"] = "College deleted successfully.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollegeModelExists(int id)
        {
            return _context.College.Any(e => e.CollegeNo == id);
        }
    }
}
