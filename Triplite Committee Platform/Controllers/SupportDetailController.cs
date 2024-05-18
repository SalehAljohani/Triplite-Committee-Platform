using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class SupportDetailController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;

        public SupportDetailController(AppDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SupportDetail
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
            return View(await _context.SupportDetail.ToListAsync());
        }

        // GET: SupportDetail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SupportDetailModel = await _context.SupportDetail
                .FirstOrDefaultAsync(m => m.Id == id);
            if (SupportDetailModel == null)
            {
                return NotFound();
            }

            return View(SupportDetailModel);
        }

        // GET: SupportDetail/Create
        public IActionResult Create()
        {
            var existingSupportDetail = _context.SupportDetail.FirstOrDefault();
            if (existingSupportDetail != null)
            {
                TempData["Message"] = "Support Page already exists, you can edit its Information below";
                return RedirectToAction("Edit", new { id = existingSupportDetail.Id });
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactEmail,PhoneNumber,TeleNumber,Location")] SupportDetail SupportDetailModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(SupportDetailModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(SupportDetailModel);
        }

        // GET: SupportDetail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SupportDetailModel = await _context.SupportDetail.FindAsync(id);
            if (SupportDetailModel == null)
            {
                return NotFound();
            }
            return View(SupportDetailModel);
        }

        // POST: SupportDetail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContactEmail,PhoneNumber,TeleNumber,Location")] SupportDetail SupportDetailModel)
        {
            if (id != SupportDetailModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(SupportDetailModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportDetailModelExists(SupportDetailModel.Id))
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
            return View(SupportDetailModel);
        }

        // GET: SupportDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SupportDetailModel = await _context.SupportDetail
                .FirstOrDefaultAsync(m => m.Id == id);
            if (SupportDetailModel == null)
            {
                return NotFound();
            }

            return View(SupportDetailModel);
        }

        // POST: SupportDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var SupportDetailModel = await _context.SupportDetail.FindAsync(id);
            if (SupportDetailModel != null)
            {
                _context.SupportDetail.Remove(SupportDetailModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool SupportDetailModelExists(int id)
        {
            return _context.SupportDetail.Any(e => e.Id == id);
        }
    }
}
