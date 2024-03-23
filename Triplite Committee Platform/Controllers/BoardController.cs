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
    public class BoardController : Controller
    {
        private readonly AppDbContext _context;

        public BoardController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Board
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Board.Include(b => b.Department).Include(b => b.RequestType).Include(b => b.Scholarship);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Board/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardModel = await _context.Board
                .Include(b => b.Department)
                .Include(b => b.RequestType)
                .Include(b => b.Scholarship)
                .FirstOrDefaultAsync(m => m.BoardNo == id);
            if (boardModel == null)
            {
                return NotFound();
            }

            return View(boardModel);
        }

        // GET: Board/Create
        public IActionResult Create()
        {
            ViewData["DeptNo"] = new SelectList(_context.Department, "DeptNo", "DeptName");
            ViewData["ReqTypeID"] = new SelectList(_context.RequestType, "RequestTypeID", "RequestTypeName");
            ViewData["National_ID"] = new SelectList(_context.Scholarship, "National_ID", "City");
            return View();
        }

        // POST: Board/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoardNo,ReqTypeID,National_ID,DeptNo,ReqStatus,ReqDate")] BoardModel boardModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(boardModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeptNo"] = new SelectList(_context.Department, "DeptNo", "DeptName", boardModel.DeptNo);
            ViewData["ReqTypeID"] = new SelectList(_context.RequestType, "RequestTypeID", "RequestTypeName", boardModel.ReqTypeID);
            ViewData["National_ID"] = new SelectList(_context.Scholarship, "National_ID", "City", boardModel.National_ID);
            return View(boardModel);
        }

        // GET: Board/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardModel = await _context.Board.FindAsync(id);
            if (boardModel == null)
            {
                return NotFound();
            }
            ViewData["DeptNo"] = new SelectList(_context.Department, "DeptNo", "DeptName", boardModel.DeptNo);
            ViewData["ReqTypeID"] = new SelectList(_context.RequestType, "RequestTypeID", "RequestTypeName", boardModel.ReqTypeID);
            ViewData["National_ID"] = new SelectList(_context.Scholarship, "National_ID", "City", boardModel.National_ID);
            return View(boardModel);
        }

        // POST: Board/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BoardNo,ReqTypeID,National_ID,DeptNo,ReqStatus,ReqDate")] BoardModel boardModel)
        {
            if (id != boardModel.BoardNo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boardModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardModelExists(boardModel.BoardNo))
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
            ViewData["DeptNo"] = new SelectList(_context.Department, "DeptNo", "DeptName", boardModel.DeptNo);
            ViewData["ReqTypeID"] = new SelectList(_context.RequestType, "RequestTypeID", "RequestTypeName", boardModel.ReqTypeID);
            ViewData["National_ID"] = new SelectList(_context.Scholarship, "National_ID", "City", boardModel.National_ID);
            return View(boardModel);
        }

        // GET: Board/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boardModel = await _context.Board
                .Include(b => b.Department)
                .Include(b => b.RequestType)
                .Include(b => b.Scholarship)
                .FirstOrDefaultAsync(m => m.BoardNo == id);
            if (boardModel == null)
            {
                return NotFound();
            }

            return View(boardModel);
        }

        // POST: Board/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boardModel = await _context.Board.FindAsync(id);
            if (boardModel != null)
            {
                _context.Board.Remove(boardModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoardModelExists(int id)
        {
            return _context.Board.Any(e => e.BoardNo == id);
        }
    }
}
