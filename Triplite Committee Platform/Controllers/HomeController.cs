using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;
using Triplite_Committee_Platform.ViewModels;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    [ValidateRole]
    public class HomeController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;

        public HomeController(UserManager<UserModel> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userDept = await _context.Department.Where(d => d.DeptNo == user.DeptNo).FirstOrDefaultAsync();
            user.Department = userDept;
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (user.EmailConfirmed == false)
            {
                TempData["Message"] = "You need to confirm your email before proceeding.";
                return RedirectToAction("Index", "ConfirmEmail");
            }

            var annoncements = await _context.Announcements.Where(a => a.DeptNo == user.DeptNo).OrderByDescending(x => x.Id).ToListAsync();

            ViewData["Announce"] = annoncements;


            var statsList = new StatsViewModel();

            var activeRole = HttpContext.Session.GetString("ActiveRole");

            if (activeRole == "Admin")
            {
                statsList = new StatsViewModel
                {
                    CompletedBoards = await _context.Board.Where(board => board.ReqStatus.ToLower() == "complete").CountAsync(),
                    CurrentBoards = await _context.Board.Where(board => board.ReqStatus.ToLower() != "complete").CountAsync(),
                    TotalBoard = await _context.Board.CountAsync()
                };
                ViewData["CardTitle"] = "Platform";
            }
            else if (activeRole == "College Dean" || activeRole == "Vice Dean")
            {
                statsList = new StatsViewModel
                {
                    CompletedBoards = await _context.Board.Where(board => board.ReqStatus.ToLower() == "complete" && board.Department.CollegeNo == user.Department.CollegeNo).CountAsync(),
                    CurrentBoards = await _context.Board.Where(board => board.ReqStatus.ToLower() != "complete" && board.Department.CollegeNo == user.Department.CollegeNo).CountAsync(),
                    TotalBoard = await _context.Board.Where(board => board.Department.CollegeNo == user.Department.CollegeNo).CountAsync()
                };
                ViewData["CardTitle"] = await _context.College.Where(c => c.CollegeNo == user.Department.CollegeNo).Select(c => c.CollegeName).FirstOrDefaultAsync();
            }
            else if (activeRole == "Head of Department" || activeRole == "Department Member")
            {
                statsList = new StatsViewModel
                {
                    CompletedBoards = await _context.Board.Where(board => board.ReqStatus.ToLower() == "complete" && board.Department.DeptNo == user.DeptNo).CountAsync(),
                    CurrentBoards = await _context.Board.Where(board => board.ReqStatus.ToLower() != "complete" && board.Department.DeptNo == user.DeptNo).CountAsync(),
                    TotalBoard = await _context.Board.Where(board => board.Department.DeptNo == user.DeptNo).CountAsync()
                };
                ViewData["CardTitle"] = await _context.Department.Where(d => d.DeptNo == user.DeptNo).Select(d => d.DeptName).FirstOrDefaultAsync();
            }
            ViewData["Stats"] = statsList;
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
