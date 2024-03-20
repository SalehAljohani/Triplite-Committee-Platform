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
			var unCompletedBoards = _context.Board.Where(board => board.ReqStatus == "Uncompleted").Count();



			//var totalCollege = _context.College.Count();



			ViewData["totalBoard"] = totalBoard;
			ViewData["CompletedBoards"] = completedBoards;
			ViewData["unCompletedBoards"] = unCompletedBoards;
			                                              
			var completionPercentage = (double)105 / 167 * 100;  //replace 105 to CompletedBoards and 167 to totalBoard

			ViewBag.CompletionPercentage = completionPercentage;


			return View();
        }
    }
}
