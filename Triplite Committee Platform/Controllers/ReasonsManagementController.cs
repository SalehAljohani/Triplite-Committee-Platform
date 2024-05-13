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
    public class ReasonsManagementController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly AppDbContext _context;

        public ReasonsManagementController(UserManager<UserModel> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
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
            return RedirectToAction("Index", "RequestManagement");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Reason not found.";
                return RedirectToAction("Index", "RequestManagement");
            }

            var reasonModel = await _context.Reasons.Include(r => r.RequestType).FirstOrDefaultAsync(m => m.ReasonID == id);
            if (reasonModel == null)
            {
                TempData["Error"] = "Reason not found.";
                return RedirectToAction("Index", "RequestManagement");
            }

            return View(reasonModel);
        }

        public async Task<IActionResult> Create(int? id)
        {
            if(id == null)
            {
                TempData["Error"] = "Request Type not found.";
                return RedirectToAction("Index", "RequestManagement");
            }
            var reqType = await _context.RequestType.FirstOrDefaultAsync(x => x.RequestTypeID == id);
            if (reqType == null)
            {
                TempData["Error"] = "Request Type not found.";
                return RedirectToAction("Index", "RequestManagement");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReqTypeID,Context,Connected")] ReasonsModel reasonsModel)
        {
            if (ModelState.IsValid)
            {
                if (_context.Reasons.Any(r => r.ReqTypeID == reasonsModel.ReqTypeID && r.Context.ToLower() == reasonsModel.Context.ToLower()))
                {
                    TempData["Error"] = "Reason already exists.";
                    return View(reasonsModel);
                }
                _context.Add(reasonsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "RequestManagement");
            }
            return View(reasonsModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Reason not found.";
                return RedirectToAction("Index", "RequestManagement");
            }

            var reasonModel = await _context.Reasons.Include(r => r.RequestType).FirstOrDefaultAsync(m => m.ReasonID == id);
            if (reasonModel == null)
            {
                TempData["Error"] = "Reason not found.";
                return RedirectToAction("Index", "RequestManagement");
            }

            return View(reasonModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReasonID,ReqTypeID,Context,Connected")] ReasonsModel reasonsModel)
        {
            if (id != reasonsModel.ReasonID)
            {
                TempData["Error"] = "Reason not found.";
                return RedirectToAction("Index", "RequestManagement");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reasonsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["Error"] = "An error occurred while updating the reason.";
                    return RedirectToAction("Index", "RequestManagement");
                }
                return RedirectToAction("Index", "RequestManagement");
            }
            return View(reasonsModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Reason not found.";
                return RedirectToAction("Index", "RequestManagement");
            }

            var reasonModel = await _context.Reasons.Include(r => r.RequestType).FirstOrDefaultAsync(m => m.ReasonID == id);
            if (reasonModel == null)
            {
                TempData["Error"] = "Reason not found.";
                return RedirectToAction("Index", "RequestManagement");
            }

            return View(reasonModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reasonModel = await _context.Reasons.FindAsync(id);
            if (reasonModel == null)
            {
                TempData["Error"] = "Reason not found.";
                return RedirectToAction("Index", "RequestManagement");
            }
            _context.Reasons.Remove(reasonModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "RequestManagement");
        }
    }
}
