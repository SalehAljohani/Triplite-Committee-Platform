using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Triplite_Committee_Platform.Data;

namespace Triplite_Committee_Platform.Controllers
{
    public class SupportController : Controller
    {
        private readonly AppDbContext _context;

        public SupportController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Requests"] = await _context.Contact.ToListAsync();
            return View(await _context.Contact.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
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

            return View(contactModel);
        }

        public async Task<IActionResult> Solved(int? id)
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
            ViewData["ButtonClass"] = "btn btn-success";
            ViewData["Button"] = "Solve";
            if(contactModel.Status == true)
            {
                ViewData["ButtonClass"] = "btn btn-danger";
                ViewData["Button"] = "Unsolve";
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
                TempData["Message"] = "Request has been marked as unsolved.";
                return RedirectToAction("Index");
            }

            contactModel.Status = true;
            _context.Update(contactModel);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Request has been marked as solved.";

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
            TempData["Message"] = "Request has been deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
