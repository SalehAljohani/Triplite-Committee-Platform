using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;
using Triplite_Committee_Platform.ViewModels;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    [ValidateRole]
    public class BoardsController : BaseController
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly AppDbContext _context;
        public BoardsController(UserManager<UserModel> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
            return View();
        }
        public IActionResult PreviousBoards()
        {
            return View();
        }
        public IActionResult CurrentBoards()
        {
            return View();
        }

        [ValidateRole("Head of Department")]
        public async Task<IActionResult> ScholarshipDetails(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Scholarship was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var reqType = await _context.RequestType.ToListAsync();
            ViewData["RequestType"] = new SelectList(reqType, "RequestTypeID", "RequestTypeName");
            var scholarshipDetails = await _context.Scholarship.FirstOrDefaultAsync(s => s.Id == id);
            return View(scholarshipDetails);
        }

        [HttpPost]
        [ValidateRole("Head of Department")]
        public async Task<IActionResult> ScholarshipDetails(ScholarshipModel model, int? ReqTypeId)
        {
            if (model == null)
            {
                TempData["Error"] = "Scholarship was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            if(ReqTypeId == null)
            {
                TempData["Error"] = "Request Type was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var reqType = await _context.RequestType.FirstOrDefaultAsync(r => r.RequestTypeID == ReqTypeId);
            if(reqType == null)
            {
                TempData["Error"] = "Request Type was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }

            model.Status = model.Status.ToLower();
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Scholarship Updated Successfully";
                var ScholarReq = new ScholarReqViewModel
                {
                    Id = model.Id,
                    ReqTypeID = reqType.RequestTypeID,
                };
                return RedirectToAction("DepartmentBoards", ScholarReq);
            }
            else
            {
                TempData["Error"] = "No Changes were made";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
        }

        [ValidateRole("Head of Department")]
        public async Task<IActionResult> DepartmentBoards(ScholarReqViewModel vModel)
        {
            if (vModel == null)
            {
                TempData["Error"] = "Scholarship was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var scholarshipDetails = await _context.Scholarship.FirstOrDefaultAsync(s => s.Id == vModel.Id);
            var reqType = await _context.RequestType.FirstOrDefaultAsync(r => r.RequestTypeID == vModel.ReqTypeID);
            var reasons = await _context.Reasons.FirstOrDefaultAsync(r => r.ReqTypeID == reqType.RequestTypeID);

            if (scholarshipDetails == null || reqType == null)
            {
                TempData["Error"] = "Error Occurred when fetching Board Details";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var ScholarReq = new ScholarReqViewModel
            {
                RequestType = reqType,
                Reason = reasons,
                Scholarship = scholarshipDetails
            };

            return View(ScholarReq);
        }


    }
}
