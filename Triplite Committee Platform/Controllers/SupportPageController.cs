using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.Controllers
{
    public class SupportPageController : Controller
    {
        private readonly AppDbContext _context;

        public SupportPageController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SupportPage
        public async Task<IActionResult> Index()
        {
            return View(await _context.SupportPage.ToListAsync());
        }

        // GET: SupportPage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportPageModel = await _context.SupportPage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supportPageModel == null)
            {
                return NotFound();
            }

            return View(supportPageModel);
        }

        // GET: SupportPage/Create
        public IActionResult Create()
        {
            var existingSupportPage = _context.SupportPage.FirstOrDefault();
            if (existingSupportPage != null)
            {
                TempData["Message"] = "Support Page already exists, you can edit its Information below";
                return RedirectToAction("Edit", new { id = existingSupportPage.Id });
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactEmail,PhoneNumber,TeleNumber,Location")] SupportPageModel supportPageModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supportPageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supportPageModel);
        }

        // GET: SupportPage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportPageModel = await _context.SupportPage.FindAsync(id);
            if (supportPageModel == null)
            {
                return NotFound();
            }
            return View(supportPageModel);
        }

        // POST: SupportPage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContactEmail,PhoneNumber,TeleNumber,Location")] SupportPageModel supportPageModel)
        {
            if (id != supportPageModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supportPageModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportPageModelExists(supportPageModel.Id))
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
            return View(supportPageModel);
        }

        // GET: SupportPage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportPageModel = await _context.SupportPage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supportPageModel == null)
            {
                return NotFound();
            }

            return View(supportPageModel);
        }

        // POST: SupportPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supportPageModel = await _context.SupportPage.FindAsync(id);
            if (supportPageModel != null)
            {
                _context.SupportPage.Remove(supportPageModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportPageModelExists(int id)
        {
            return _context.SupportPage.Any(e => e.Id == id);
        }
    }
}
