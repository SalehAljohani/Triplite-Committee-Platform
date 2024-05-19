using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateRole("Admin")]
    public class SupportController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly IStringLocalizer<SupportController> Localizer;

        public SupportController(AppDbContext context, UserManager<UserModel> userManager, IStringLocalizer<SupportController> localizer)
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
            return View(await _context.Contact.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                string reqNotFound = Localizer["reqNotFound"];
                TempData["Message"] = reqNotFound;
                return RedirectToAction("Index");
            }

            var contactModel = await _context.Contact.FirstOrDefaultAsync(m => m.Id == id);
            if (contactModel == null)
            {
                string reqNotFound = Localizer["reqNotFound"];
                TempData["Message"] = reqNotFound;
                return RedirectToAction("Index");
            }

            return View(contactModel);
        }

        public async Task<IActionResult> Solved(int? id)
        {
            if (id == null)
            {
                string reqNotFound = Localizer["reqNotFound"];
                TempData["Message"] = reqNotFound;
                return RedirectToAction("Index");
            }

            var contactModel = await _context.Contact.FirstOrDefaultAsync(m => m.Id == id);
            if (contactModel == null)
            {
                string reqNotFound = Localizer["reqNotFound"];
                TempData["Message"] = reqNotFound;
                return RedirectToAction("Index");
            }
            ViewData["ButtonClass"] = "btn btn-success";
            string solve = Localizer["solve"];
            ViewData["Button"] = solve;
            if(contactModel.Status == true)
            {
                ViewData["ButtonClass"] = "btn btn-danger";
                string unsolve = Localizer["unsolve"];
                ViewData["Button"] = unsolve;
            }

            return View(contactModel);
        }

        public async Task<IActionResult> SolveConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactModel = await _context.Contact.FirstOrDefaultAsync(m => m.Id == id);
            if (contactModel == null)
            {
                return NotFound();
            }
            if (contactModel.Status == true)
            {
                contactModel.Status = false;
                _context.Update(contactModel);
                await _context.SaveChangesAsync();
                string reqUnsolved = Localizer["reqUnsolved"];
                TempData["Message"] = reqUnsolved;
                return RedirectToAction("Index");
            }

            contactModel.Status = true;
            _context.Update(contactModel);
            await _context.SaveChangesAsync();
            string reqSolved = Localizer["reqSolved"];
            TempData["Message"] = reqSolved;

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactModel = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactModel == null)
            {
                return NotFound();
            }

            return View(contactModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactModel = await _context.Contact.FindAsync(id);
            _context.Contact.Remove(contactModel);
            await _context.SaveChangesAsync();
            string reqDeleted = Localizer["reqDeleted"];
            TempData["Message"] = reqDeleted;
            return RedirectToAction(nameof(Index));
        }
    }
}
