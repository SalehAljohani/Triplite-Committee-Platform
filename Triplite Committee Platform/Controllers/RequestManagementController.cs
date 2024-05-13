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
    public class RequestManagementController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;

        public RequestManagementController(AppDbContext context, UserManager<UserModel> userManager)
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

            var requestType = await _context.RequestType.Include(r => r.Reasons).ToListAsync();

            return View(requestType);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Request Type Not Found.";
                return RedirectToAction("Index");
            }

            var requestType = await _context.RequestType.Include(r => r.Reasons).FirstOrDefaultAsync(m => m.RequestTypeID == id);

            if (requestType == null)
            {
                TempData["Error"] = "Request Type Not Found.";
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
                _context.Add(requestTypeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(requestTypeModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Request Type Not Found.";
                return RedirectToAction("Index");
            }

            var requestTypeModel = await _context.RequestType.FindAsync(id);

            if (requestTypeModel == null)
            {
                TempData["Error"] = "Request Type Not Found.";
            }

            return View(requestTypeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestTypeName")] RequestTypeModel requestTypeModel)
        {
            if (id != requestTypeModel.RequestTypeID)
            {
                TempData["Error"] = "Request Type Not Found.";
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
                        TempData["Error"] = "Request Type Not Found.";
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
                TempData["Error"] = "Request Type Not Found.";
                return RedirectToAction("Index");
            }

            var requestTypeModel = await _context.RequestType.FirstOrDefaultAsync(m => m.RequestTypeID == id);

            if (requestTypeModel == null)
            {
                TempData["Error"] = "Request Type Not Found.";
            }

            var existingReasons = _context.Reasons.Where(d => d.ReqTypeID == id).ToList();
            if (existingReasons.Any())
            {
                TempData["DeleteReasons"] = true;
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
                    foreach (var reason in existingReasons)
                    {
                        _context.Reasons.Remove(reason);
                    }
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
