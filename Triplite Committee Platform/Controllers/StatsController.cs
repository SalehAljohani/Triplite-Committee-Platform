using Microsoft.AspNetCore.Mvc;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;



namespace Triplite_Committee_Platform.Controllers
{
    public class StatsController : Controller
    {

        private readonly AppDbContext _context;

        public StatsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalBoard = _context.Board.Count();
			var completedBoards = _context.Board.Where(board => board.ReqStatus == "Completed").Count();
			var currentBoards = _context.Board.Where(board => board.ReqStatus == "Uncompleted").Count();



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
           // to here _________________________________________________________________________________________________________________//|


            ViewData["totalBoard"] = totalBoard;
			ViewData["completedBoards"] = completedBoards;
			ViewData["currentBoards"] = currentBoards;
			                                              
			var completionPercentage = (double)105 / 167 * 100;  //replace 105 to CompletedBoards and 167 to totalBoard

			ViewBag.CompletionPercentage = completionPercentage;


			return View();
        }
    }
}
