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
    public class StatsController : Controller
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
            var userEmail = await _userManager.GetUserAsync(User);
            if (userEmail.EmailConfirmed == false)
            {
                return RedirectToAction("Index", "ConfirmEmail");
            }
            //var totalBoard = await _context.Board.CountAsync();
            //var completedBoards = await _context.Board.Where(board => board.ReqStatus == "Completed").CountAsync();
            //var currentBoards = await _context.Board.Where(board => board.ReqStatus == "Uncompleted").CountAsync();

            //ViewData["totalBoard"] = totalBoard;
            //ViewData["completedBoards"] = completedBoards;
            //ViewData["currentBoards"] = currentBoards;

            //var completionPercentage = (double) 105 / 167 * 100;  // replace 105 to CompletedBoards and 167 to totalBoard
            //ViewBag.CompletionPercentage = completionPercentage;



            var totalCollege = _context.College.ToList();
            ViewData["totalCollege"] = totalCollege;
            // Under test from here ______________________________________________________________________________________________________
            foreach (var college in totalCollege)                                                                                      //|
            {                                                                                                                          //|
                var collegeCompletedBoards = _context.Board                                                                            //|
                    .Where(board => board.Department.CollegeNo == college.CollegeNo && board.ReqStatus == "Completed")                 //|
                    .Count(); // Count the completed boards for each college                                                           //|
                ViewData[$"completedBoards_{college.CollegeNo}"] = collegeCompletedBoards;                                             //|
                var collegeCurrentBoards = _context.Board                                                                              //|
                    .Where(board => board.Department.CollegeNo == college.CollegeNo && board.ReqStatus == "Uncompleted")               //|
                    .Count();          // Count the current boards for each college                                                    //|
                ViewData[$"currentBoards_{college.CollegeNo}"] = collegeCurrentBoards;                                                 //|
                var collegeTotalBoards = _context.Board                                                                                //|
                    .Where(board => board.Department.CollegeNo == college.CollegeNo)                                                   //|
                    .Count();         // Count the total boards for each college                                                       //|
                ViewData[$"totalBoards_{college.CollegeNo}"] = collegeTotalBoards; // Store the completed boards count in ViewData     //|
                                                                                                                                       //|
            }                                                                                                                          //|
            // to here ________________________________________________________________________________________________________________//|


            return View();
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
            }else
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
