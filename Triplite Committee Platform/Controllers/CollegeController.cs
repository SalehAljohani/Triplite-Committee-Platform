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
    public class CollegeController : Controller
    {
        private readonly AppDbContext _context;

        public CollegeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ManageCollege
        public async Task<IActionResult> Index()
        {
            return View(await _context.College.ToListAsync());
        }

        // GET: ManageCollege/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collegeModel = await _context.College
                .FirstOrDefaultAsync(m => m.CollegeNo == id);
            if (collegeModel == null)
            {
                return NotFound();
            }

            return View(collegeModel);
        }

        // GET: ManageCollege/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageCollege/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CollegeName")] CollegeModel collegeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(collegeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(collegeModel);
        }

        // GET: ManageCollege/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collegeModel = await _context.College.FindAsync(id);
            if (collegeModel == null)
            {
                return NotFound();
            }
            return View(collegeModel);
        }

        // POST: ManageCollege/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CollegeNo,CollegeName")] CollegeModel collegeModel)
        {
            if (id != collegeModel.CollegeNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(collegeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollegeModelExists(collegeModel.CollegeNo))
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
            return View(collegeModel);
        }

        // GET: ManageCollege/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collegeModel = await _context.College
                .FirstOrDefaultAsync(m => m.CollegeNo == id);
            if (collegeModel == null)
            {
                return NotFound();
            }

            return View(collegeModel);
        }

        // POST: ManageCollege/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collegeModel = await _context.College.FindAsync(id);
            if (collegeModel != null)
            {
                _context.College.Remove(collegeModel);
                TempData["DeleteMessage"] = "College deleted successfully.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollegeModelExists(int id)
        {
            return _context.College.Any(e => e.CollegeNo == id);
        }
    }
}
