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
    [Authorize(Roles = "Admin")]
    [ValidateRole("Admin")]
    public class DepartmentController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;

        public DepartmentController(AppDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Department
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

        // GET: Department/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentModel = await _context.Department
                .Include(d => d.College)
                .FirstOrDefaultAsync(m => m.DeptNo == id);
            if (departmentModel == null)
            {
                return NotFound();
            }

            return View(departmentModel);
        }

        // GET: Department/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if(_context.College.Find(id) == null)
            {
                return NotFound();
            }
            ViewData["CollegeNo"] = new SelectList(_context.College, "CollegeNo", "CollegeName", id);
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return NotFound();
            }

            var departmentModel = await _context.Department.FindAsync(id);
            if (departmentModel == null)
            {
                return NotFound();
            }
            ViewData["CollegeNo"] = new SelectList(_context.College, "CollegeNo", "CollegeName", departmentModel.CollegeNo);
            return View(departmentModel);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CollegeNo,DeptName,DeptNo")] DepartmentModel departmentModel)
        {
            if (id != departmentModel.DeptNo)
            {
                return NotFound();
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
                        return NotFound();
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

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["Message"] = "Department not found.";
                return View();
            }

            if(_context.Department.Find(id) == null)
            {
                TempData["Message"] = "Department not found.";
                return View();
            }

            var departmentModel = await _context.Department
                .Include(d => d.College)
                .FirstOrDefaultAsync(m => m.DeptNo == id);
            if (departmentModel == null)
            {
                TempData["Message"] = "Department not found.";
                return View();
            }

            var hasUsers = await _context.User.AnyAsync(u => u.DeptNo == id);

            if (hasUsers)
            {
                ViewData["UserMessage"] = "Department has users. Please delete users first.";
                
                return View(departmentModel);
            }

            return View(departmentModel);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departmentModel = await _context.Department.FindAsync(id);
            var hasUsers = await _context.User.AnyAsync(u => u.DeptNo == id);

            if (hasUsers)
            {
                TempData["UserMessage"] = "Department has users. Please delete users first.";

                return View(departmentModel);
            }
            if (departmentModel != null)
            {
                _context.Department.Remove(departmentModel);
                ViewData["Message"] = "Department deleted successfully.";
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
