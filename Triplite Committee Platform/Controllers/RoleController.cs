using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public RoleController(RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        public IList<IdentityRole> Roles { get; set; }

        // GET: Role List
        public async Task<IActionResult> Index()
        {
            var role = await _roleManager.Roles.ToListAsync();
            return View(role);
        }
        // GET: Role Details
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
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
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
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
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var existingRole = await _roleManager.FindByIdAsync(role.Id);
                    if (existingRole == null)
                    {
                        return NotFound();
                    }

                    existingRole.Name = role.Name;
                    await _roleManager.UpdateAsync(existingRole);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.Id))
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
            return View(role);
        }

        // GET: Role Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
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
            }
            return RedirectToAction(nameof(Index));
        }
        private bool RoleExists(string id)
        {
            return _roleManager.Roles.Any(r => r.Id == id);
        }
    }
}
