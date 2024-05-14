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
    public class RoleController : BaseController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly IStringLocalizer<RoleController> Localizer;

        public RoleController(RoleManager<IdentityRole> roleManager, AppDbContext context, UserManager<UserModel> userManager, IStringLocalizer<RoleController>localizer)
        {
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
            Localizer = localizer;
        }
        public IList<IdentityRole> Roles { get; set; }

        // GET: Role List
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
            var role = await _roleManager.Roles.ToListAsync();
            return View(role);
        }
        // GET: Role Details
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                TempData["Error"] = @Localizer["roleNotFound"];
                return RedirectToAction(nameof(Index));
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["Error"] = @Localizer["roleNotFound"];
                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }
        // GET: Role Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }
        // POST: Role Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }
        // GET: Role Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                TempData["Error"] = @Localizer["roleNotFound"];
                return RedirectToAction(nameof(Index));
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["Error"] = @Localizer["roleNotFound"];
                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }
        // POST: Role Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] IdentityRole role)
        {
            if (id != role.Id)
            {
                TempData["Error"] = @Localizer["roleNotFound"];
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var existingRole = await _roleManager.FindByIdAsync(role.Id);
                    if (existingRole == null)
                    {
                        TempData["Error"] = @Localizer["roleNotFound"];
                        return RedirectToAction(nameof(Index));
                    }

                    existingRole.Name = role.Name;
                    await _roleManager.UpdateAsync(existingRole);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.Id))
                    {
                        TempData["Error"] = @Localizer["roleNotFound"];
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Role Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                TempData["Error"] = @Localizer["roleNotFound"];
                return RedirectToAction(nameof(Index));
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                TempData["Error"] = @Localizer["roleNotFound"];
                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }
        // POST: Role Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
                TempData["DeleteMessage"] = @Localizer["roleDelete"];
            }
            return RedirectToAction(nameof(Index));
        }
        private bool RoleExists(string id)
        {
            return _roleManager.Roles.Any(r => r.Id == id);
        }
    }
}
