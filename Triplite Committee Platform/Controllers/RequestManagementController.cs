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
    public class RequestManagementController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly IStringLocalizer<RequestManagementController> Localizer;

        public RequestManagementController(AppDbContext context, UserManager<UserModel> userManager, IStringLocalizer<RequestManagementController> localizer)
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

            var requestType = await _context.RequestType.Include(r => r.Reasons).ToListAsync();

            return View(requestType);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                string reqTypeNotFound = @Localizer["reqTypeNotFound"];
                TempData["Error"] = reqTypeNotFound;
                return RedirectToAction("Index");
            }

            var requestType = await _context.RequestType.Include(r => r.Reasons).FirstOrDefaultAsync(m => m.RequestTypeID == id);

            if (requestType == null)
            {
                string reqTypeNotFound = @Localizer["reqTypeNotFound"];
                TempData["Error"] = reqTypeNotFound;
            }

            return View(requestType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestTypeName")] RequestTypeModel requestTypeModel)
        {
            if (ModelState.IsValid)
            {
                if(_context.RequestType.Any(r => r.RequestTypeName == requestTypeModel.RequestTypeName))
                {
                    string reqTypeExist = @Localizer["reqTypeExist"];
                    TempData["Error"] = reqTypeExist;
                    return View(requestTypeModel);
                }
                _context.Add(requestTypeModel);
                await _context.SaveChangesAsync();
                string reqTypeCreated = @Localizer["reqTypeCreated"];
                TempData["Success"] = reqTypeCreated;
                return RedirectToAction(nameof(Index));
            }
            return View(requestTypeModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                string reqTypeNotFound = @Localizer["reqTypeNotFound"];
                TempData["Error"] = reqTypeNotFound;
                return RedirectToAction("Index");
            }

            var requestTypeModel = await _context.RequestType.FindAsync(id);

            if (requestTypeModel == null)
            {
                string reqTypeNotFound = @Localizer["reqTypeNotFound"];
                TempData["Error"] = reqTypeNotFound;
            }

            return View(requestTypeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestTypeName")] RequestTypeModel requestTypeModel)
        {
            if (id != requestTypeModel.RequestTypeID)
            {
                string reqTypeNotFound = @Localizer["reqTypeNotFound"];
                TempData["Error"] = reqTypeNotFound;
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestTypeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestTypeModelExists(requestTypeModel.RequestTypeID))
                    {
                        string reqTypeNotFound = @Localizer["reqTypeNotFound"];
                        TempData["Error"] = reqTypeNotFound;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(requestTypeModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                string reqTypeNotFound = @Localizer["reqTypeNotFound"];
                TempData["Error"] = reqTypeNotFound;
                return RedirectToAction("Index");
            }

            var requestTypeModel = await _context.RequestType.FirstOrDefaultAsync(m => m.RequestTypeID == id);

            if (requestTypeModel == null)
            {
                string reqTypeNotFound = @Localizer["reqTypeNotFound"];
                TempData["Error"] = reqTypeNotFound;
            }
            return View(requestTypeModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, bool deleteReasons)
        {
            var requestTypeModel = await _context.RequestType.FindAsync(id);
            if (deleteReasons)
            {
                var existingReasons = _context.Reasons.Where(d => d.ReqTypeID == id).ToList();
                if (existingReasons.Any())
                {
                    string reasonsConnected = @Localizer["reasonsConnected"];
                    TempData["Error"] = reasonsConnected;
                    return RedirectToAction(nameof(Index));
                }
            }
            _context.RequestType.Remove(requestTypeModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestTypeModelExists(int id)
        {
            return _context.RequestType.Any(e => e.RequestTypeID == id);
        }
    }
}
