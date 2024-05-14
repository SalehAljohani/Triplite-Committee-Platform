using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.ViewModels;




namespace Triplite_Committee_Platform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatsController : BaseController
    {

        private readonly AppDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public StatsController(AppDbContext context, UserManager<UserModel> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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

            var statsList = new List<StatsViewModel>();


            var activeRole = HttpContext.Session.GetString("ActiveRole");

            if (activeRole == "Admin")
            {
                var college = await _context.College.ToListAsync();
                var department = await _context.Department.ToListAsync();

                foreach (var col in college)
                {
                    var stats = new StatsViewModel
                    {
                        College = col.CollegeName,
                        Departments = new List<DepartmentStatsViewModel>(),
                        CompletedBoards = await _context.Board.Where(board => board.ReqStatus == "Completed" && board.Department.CollegeNo == col.CollegeNo).CountAsync(),
                        CurrentBoards = await _context.Board.Where(board => board.ReqStatus == "Uncompleted" && board.Department.CollegeNo == col.CollegeNo).CountAsync(),
                        TotalBoard = await _context.Board.Where(board => board.Department.CollegeNo == col.CollegeNo).CountAsync()
                    };
                    var departments = await _context.Department.Where(d => d.CollegeNo == col.CollegeNo).ToListAsync();
                    foreach (var dept in departments)
                    {
                        var deptStats = new DepartmentStatsViewModel
                        {
                            DepartmentName = dept.DeptName,
                            CompletedBoards = await _context.Board.Where(board => board.ReqStatus == "Completed" && board.DeptNo == dept.DeptNo).CountAsync(),
                            CurrentBoards = await _context.Board.Where(board => board.ReqStatus == "Uncompleted" && board.DeptNo == dept.DeptNo).CountAsync(),
                            TotalBoard = await _context.Board.Where(board => board.DeptNo == dept.DeptNo).CountAsync()
                        };
                        stats.Departments.Add(deptStats);
                    }

                    statsList.Add(stats);
                }
            }
            return View(statsList);
        }
        public async Task<PartialViewResult> GetStats() // should be edited to show stats based on the user's role now it's show stats for (Admin, User) which are not real roles
        {

            var selectedRole = _httpContextAccessor.HttpContext.Request.Cookies["SelectedRole"];

            if (selectedRole == "Admin")
            {
                var model = new StatsViewModel
                {
                    CompletedBoards = await _context.Board.Where(board => board.ReqStatus == "Completed").CountAsync(),
                    CurrentBoards = await _context.Board.Where(board => board.ReqStatus == "Uncompleted").CountAsync(),
                    TotalBoard = await _context.Board.CountAsync()
                };
                return PartialView("_StatsPartial", model);
            }
            else if (selectedRole == "User")
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var model = new StatsViewModel
                    {
                        CompletedBoards = await _context.Board.Where(board => board.ReqStatus == "Completed" && board.DeptNo == user.DeptNo).CountAsync(),
                        CurrentBoards = await _context.Board.Where(board => board.ReqStatus == "Uncompleted" && board.DeptNo == user.DeptNo).CountAsync(),
                        TotalBoard = await _context.Board.Where(board => board.DeptNo == user.DeptNo).CountAsync()
                    };
                    return PartialView("_StatsPartial", model);
                }
            }
            else
            {
                var model = new StatsViewModel
                {
                    CompletedBoards = await _context.Board.Where(board => board.ReqStatus == "Completed").CountAsync(),
                    CurrentBoards = await _context.Board.Where(board => board.ReqStatus == "Uncompleted").CountAsync(),
                    TotalBoard = await _context.Board.CountAsync()
                };
                return PartialView("_StatsPartial", model);
            }

            return PartialView("_StatsPartial", new StatsViewModel()); // return an empty model if no conditions are met
        }
    }
}
