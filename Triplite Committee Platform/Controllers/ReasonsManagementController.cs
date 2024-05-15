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
    public class ReasonsManagementController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<ReasonsManagementController> Localizer;

        public ReasonsManagementController(UserManager<UserModel> userManager, AppDbContext context, IStringLocalizer<ReasonsManagementController>localizer)
        {
            _userManager = userManager;
            _context = context;
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
            return RedirectToAction("Index", "RequestManagement");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                string reasonNotFound = @Localizer["reasonNotFound"];
                TempData["Error"] = reasonNotFound;
                return RedirectToAction("Index", "RequestManagement");
            }

            var reasonModel = await _context.Reasons.Include(r => r.RequestType).FirstOrDefaultAsync(m => m.ReasonID == id);
            if (reasonModel == null)
            {
                string reasonNotFound = @Localizer["reasonNotFound"];
                TempData["Error"] = reasonNotFound;
                return RedirectToAction("Index", "RequestManagement");
            }

            return View(reasonModel);
        }

        public async Task<IActionResult> Create(int? id)
        {
            if(id == null)
            {
                string reqTypeNotFound = @Localizer["reqTypeNotFound"];
                TempData["Error"] = reqTypeNotFound;
                return RedirectToAction("Index", "RequestManagement");
            }
            var reqType = await _context.RequestType.FirstOrDefaultAsync(x => x.RequestTypeID == id);
            if (reqType == null)
            {
                string reqTypeNotFound = @Localizer["reqTypeNotFound"];
                TempData["Error"] = reqTypeNotFound;
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
                    string reasonExist = @Localizer["reasonExist"];
                    TempData["Error"] = reasonExist;
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
                string reasonNotFound = @Localizer["reasonNotFound"];
                TempData["Error"] = reasonNotFound;
                return RedirectToAction("Index", "RequestManagement");
            }

            var reasonModel = await _context.Reasons.Include(r => r.RequestType).FirstOrDefaultAsync(m => m.ReasonID == id);
            if (reasonModel == null)
            {
                string reasonNotFound = @Localizer["reasonNotFound"];
                TempData["Error"] = reasonNotFound;
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
                string reasonNotFound = @Localizer["reasonNotFound"];
                TempData["Error"] = reasonNotFound;
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
                    string updateError = @Localizer["updateError"];
                    TempData["Error"] = updateError;
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
                string reasonNotFound = @Localizer["reasonNotFound"];
                TempData["Error"] = reasonNotFound;
                return RedirectToAction("Index", "RequestManagement");
            }

            var reasonModel = await _context.Reasons.Include(r => r.RequestType).FirstOrDefaultAsync(m => m.ReasonID == id);
            if (reasonModel == null)
            {
                string reasonNotFound = @Localizer["reasonNotFound"];
                TempData["Error"] = reasonNotFound;
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
                string reasonNotFound = @Localizer["reasonNotFound"];
                TempData["Error"] = reasonNotFound;
                return RedirectToAction("Index", "RequestManagement");
            }
            _context.Reasons.Remove(reasonModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "RequestManagement");
        }
    }
}
