using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;
using Triplite_Committee_Platform.ViewModels;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    [ValidateRole]
    public class BoardsController : BaseController
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly AppDbContext _context;
        private readonly IConverter _converter;
        public BoardsController(UserManager<UserModel> userManager, AppDbContext context, IConverter converter)
        {
            _userManager = userManager;
            _context = context;
            _converter = converter;
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
            return View();
        }
        public async Task<IActionResult> PreviousBoards()
        {
            var boards = new List<BoardModel>();
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var activeRole = HttpContext.Session.GetString("ActiveRole");
            if (activeRole == "Admin")
            {
                var userDept = await _context.Department.Where(d => d.DeptNo == user.DeptNo).FirstOrDefaultAsync();
                if (userDept == null)
                {
                    TempData["Error"] = "You are not Authorized";
                    return RedirectToAction("Index", "Home");
                }
                var college = await _context.College.Where(c => c.CollegeNo == userDept.CollegeNo).FirstOrDefaultAsync();
                var departments = await _context.Department.Where(d => d.CollegeNo == college.CollegeNo).ToListAsync();

                foreach (var department in departments)
                {
                    var departmentBoards = await _context.Board.Where(b => b.ReqStatus.ToLower() == "completed" && b.DeptNo == department.DeptNo).ToListAsync();
                    boards.AddRange(departmentBoards);
                }

                foreach (var item in boards)
                {
                    var scholar = await _context.Scholarship.Where(s => s.Id == item.Id).FirstOrDefaultAsync();
                    var reqType = await _context.RequestType.Where(r => r.RequestTypeID == item.ReqTypeID).FirstOrDefaultAsync();
                    item.Scholarship = scholar;
                    item.RequestType = reqType;

                    if (item.ReqStatus.ToLower() == "college")
                    {
                        ViewData["CollegeBoard"] = true;
                    }
                    else
                    {
                        ViewData["CollegeBoard"] = false;
                    }
                }
            }
            if (activeRole == "Vice Dean" || activeRole == "College Dean" || activeRole == "Head of Department")
            {
                var userDept = await _context.Department.Where(d => d.DeptNo == user.DeptNo).FirstOrDefaultAsync();
                if (userDept == null)
                {
                    TempData["Error"] = "You are not Authorized";
                    return RedirectToAction("Index", "Home");
                }
                var college = await _context.College.Where(c => c.CollegeNo == userDept.CollegeNo).FirstOrDefaultAsync();
                var departments = await _context.Department.Where(d => d.CollegeNo == college.CollegeNo).ToListAsync();

                foreach (var department in departments)
                {
                    var departmentBoards = await _context.Board.Where(b => b.ReqStatus.ToLower() == "completed" && b.DeptNo == department.DeptNo).ToListAsync();
                    boards.AddRange(departmentBoards);
                }

                foreach (var item in boards)
                {
                    var scholar = await _context.Scholarship.Where(s => s.Id == item.Id).FirstOrDefaultAsync();
                    var reqType = await _context.RequestType.Where(r => r.RequestTypeID == item.ReqTypeID).FirstOrDefaultAsync();
                    item.Scholarship = scholar;
                    item.RequestType = reqType;

                    if (item.ReqStatus.ToLower() == "college")
                    {
                        ViewData["CollegeBoard"] = true;
                    }
                    else
                    {
                        ViewData["CollegeBoard"] = false;
                    }
                }
            }
            if (activeRole == "Head of Department" || activeRole == "Department Member")
            {
                boards = await _context.Board.Where(b => b.ReqStatus.ToLower() == "completed" && b.DeptNo == user.DeptNo).ToListAsync();
                foreach (var item in boards)
                {
                    var scholar = await _context.Scholarship.Where(s => s.Id == item.Id).FirstOrDefaultAsync();
                    var reqType = await _context.RequestType.Where(r => r.RequestTypeID == item.ReqTypeID).FirstOrDefaultAsync();
                    var dept = await _context.Department.Where(d => d.DeptNo == item.DeptNo).FirstOrDefaultAsync();
                    item.Scholarship = scholar;
                    item.RequestType = reqType;
                    item.Department = dept;
                    if (item.ReqStatus.ToLower() == "college")
                    {
                        ViewData["CollegeBoard"] = true;
                    }
                    else
                    {
                        ViewData["CollegeBoard"] = false;
                    }
                }
                if (boards.Count == 0)
                {
                    if (activeRole == "Vice Dean" || activeRole == "College Dean" || activeRole == "Head of Department")
                    {
                        var userDept = await _context.Department.Where(d => d.DeptNo == user.DeptNo).FirstOrDefaultAsync();
                        if (userDept == null)
                        {
                            TempData["Error"] = "You are not Authorized";
                            return RedirectToAction("Index", "Home");
                        }
                        var college = await _context.College.Where(c => c.CollegeNo == userDept.CollegeNo).FirstOrDefaultAsync();
                        var departments = await _context.Department.Where(d => d.CollegeNo == college.CollegeNo).ToListAsync();

                        foreach (var department in departments)
                        {
                            var departmentBoards = await _context.Board.Where(b => b.ReqStatus.ToLower() == "completed" && b.DeptNo == department.DeptNo).ToListAsync();
                            boards.AddRange(departmentBoards);
                        }

                        foreach (var item in boards)
                        {
                            var scholar = await _context.Scholarship.Where(s => s.Id == item.Id).FirstOrDefaultAsync();
                            var reqType = await _context.RequestType.Where(r => r.RequestTypeID == item.ReqTypeID).FirstOrDefaultAsync();
                            item.Scholarship = scholar;
                            item.RequestType = reqType;

                            if (item.ReqStatus.ToLower() == "college")
                            {
                                ViewData["CollegeBoard"] = true;
                            }
                            else
                            {
                                ViewData["CollegeBoard"] = false;
                            }
                        }
                    }
                }
            }
            return View(boards);
        }

        [ValidateRole]
        public async Task<IActionResult> CurrentBoards()
        {
            var boards = new List<BoardModel>();
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var activeRole = HttpContext.Session.GetString("ActiveRole");

            if (activeRole == "Vice Dean" || activeRole == "College Dean" || activeRole == "Head of Department")
            {
                var userDept = await _context.Department.Where(d => d.DeptNo == user.DeptNo).FirstOrDefaultAsync();
                if (userDept == null)
                {
                    TempData["Error"] = "You are not Authorized";
                    return RedirectToAction("Index", "Home");
                }
                var college = await _context.College.Where(c => c.CollegeNo == userDept.CollegeNo).FirstOrDefaultAsync();
                var departments = await _context.Department.Where(d => d.CollegeNo == college.CollegeNo).ToListAsync();
                if (activeRole == "Head of Department")
                {
                    foreach (var department in departments)
                    {
                        var departmentBoards = await _context.Board.Where(b => (b.ReqStatus.ToLower() == "college" || b.ReqStatus.ToLower() == "department") && b.DeptNo == department.DeptNo).ToListAsync();
                        boards.AddRange(departmentBoards);
                    }
                    foreach (var item in boards)
                    {
                        var scholar = await _context.Scholarship.Where(s => s.Id == item.Id).FirstOrDefaultAsync();
                        var reqType = await _context.RequestType.Where(r => r.RequestTypeID == item.ReqTypeID).FirstOrDefaultAsync();
                        item.Scholarship = scholar;
                        item.RequestType = reqType;

                        if (item.ReqStatus.ToLower() == "college")
                        {
                            ViewData["CollegeBoard"] = true;
                        }
                        else
                        {
                            ViewData["CollegeBoard"] = false;
                        }
                    }
                }
                else
                {
                    foreach (var department in departments)
                    {
                        var departmentBoards = await _context.Board.Where(b => b.ReqStatus.ToLower() == "college" && b.DeptNo == department.DeptNo).ToListAsync();
                        boards.AddRange(departmentBoards);
                    }
                    foreach (var item in boards)
                    {
                        var scholar = await _context.Scholarship.Where(s => s.Id == item.Id).FirstOrDefaultAsync();
                        var reqType = await _context.RequestType.Where(r => r.RequestTypeID == item.ReqTypeID).FirstOrDefaultAsync();
                        item.Scholarship = scholar;
                        item.RequestType = reqType;

                        if (item.ReqStatus.ToLower() == "college")
                        {
                            ViewData["CollegeBoard"] = true;
                        }
                        else
                        {
                            ViewData["CollegeBoard"] = false;
                        }
                    }
                }
            }
            if (boards.Count == 0)
            {
                if (activeRole == "Head of Department" || activeRole == "Department Member")
                {
                    boards = await _context.Board.Where(b => b.ReqStatus.ToLower() == "department" && b.DeptNo == user.DeptNo).ToListAsync();
                    foreach (var item in boards)
                    {
                        var scholar = await _context.Scholarship.Where(s => s.Id == item.Id).FirstOrDefaultAsync();
                        var reqType = await _context.RequestType.Where(r => r.RequestTypeID == item.ReqTypeID).FirstOrDefaultAsync();
                        var dept = await _context.Department.Where(d => d.DeptNo == item.DeptNo).FirstOrDefaultAsync();
                        item.Scholarship = scholar;
                        item.RequestType = reqType;
                        item.Department = dept;
                        if (item.ReqStatus.ToLower() == "college")
                        {
                            ViewData["CollegeBoard"] = true;
                        }
                        else
                        {
                            ViewData["CollegeBoard"] = false;
                        }
                    }
                }
            }
            return View(boards);
        }

        [ValidateRole]
        public async Task<IActionResult> ScholarshipDetails(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Scholarship was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var scholarshipDetails = await _context.Scholarship.FirstOrDefaultAsync(s => s.Id == id);
            if (scholarshipDetails == null)
            {
                TempData["Error"] = "Scholarship was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var board = await _context.Board.FirstOrDefaultAsync(b => b.Id == id);
            if (board != null)
            {
                if (board.ReqStatus.ToLower() == "college")
                    ViewData["CollegeBoard"] = true;
            }
            else
            {
                ViewData["CollegeBoard"] = false;
            }
            var reqType = await _context.RequestType.Where(r => r.RequestTypeID.ToString() == scholarshipDetails.Status).FirstOrDefaultAsync();
            if (reqType != null)
            {
                ViewData["RequestType"] = reqType;
            }
            else
            {
                reqType = await _context.RequestType.Where(r => r.RequestTypeID == board.ReqTypeID).FirstOrDefaultAsync();
                ViewData["RequestType"] = reqType;
            }
            return View(scholarshipDetails);
        }

        [HttpPost]
        [ValidateRole("Head of Department", "Department Member")]
        public async Task<IActionResult> ScholarshipDetails(ScholarshipModel model, int? ReqTypeId)
        {
            if (model == null)
            {
                TempData["Error"] = "Scholarship was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            if (ReqTypeId == null)
            {
                TempData["Error"] = "Request Type was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var reqType = await _context.RequestType.FirstOrDefaultAsync(r => r.RequestTypeID == ReqTypeId);
            if (reqType == null)
            {
                TempData["Error"] = "Request Type was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Scholarship Updated Successfully";
                var ScholarReq = new BoardDetailsViewModel
                {
                    Id = model.Id,
                    ReqTypeID = reqType.RequestTypeID,
                };
                return RedirectToAction("DepartmentBoards", ScholarReq);
            }
            else
            {
                TempData["Error"] = "No Changes were made";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
        }

        [ValidateRole("Head of Department", "Department Member")]
        public async Task<IActionResult> DepartmentBoards(BoardDetailsViewModel vModel)
        {
            if (vModel == null)
            {
                TempData["Error"] = "Scholarship was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var scholarshipDetails = await _context.Scholarship.FirstOrDefaultAsync(s => s.Id == vModel.Id);
            var reqType = await _context.RequestType.FirstOrDefaultAsync(r => r.RequestTypeID == vModel.ReqTypeID);
            var reasons = await _context.Reasons.FirstOrDefaultAsync(r => r.ReqTypeID == reqType.RequestTypeID);

            if (scholarshipDetails == null || reqType == null)
            {
                TempData["Error"] = "Error Occurred when fetching Board Details";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var board = await _context.Board.FirstOrDefaultAsync(b => b.Id == vModel.Id && b.ReqTypeID == vModel.ReqTypeID);
            var ScholarReq = new BoardDetailsViewModel();
            if (board != null)
            {
                ScholarReq = new BoardDetailsViewModel
                {
                    Board = board,
                    Scholarship = scholarshipDetails,
                    RequestType = reqType,
                };
                return View(ScholarReq);
            }
            string context = reasons.Context;
            string contextFormated = context.Replace(Environment.NewLine, "<br />");
            ViewData["rContext"] = contextFormated;
            ScholarReq = new BoardDetailsViewModel
            {
                RequestType = reqType,
                Reason = reasons,
                Board = null,
                Scholarship = scholarshipDetails
            };

            return View(ScholarReq);
        }

        [ValidateRole("College Dean", "Vice Dean", "Head of Department")]
        public async Task<IActionResult> RedirectedDetails(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardNo == id);
            var request = await _context.RequestType.FirstOrDefaultAsync(r => r.RequestTypeID == board.ReqTypeID);
            var scholarship = await _context.Scholarship.FirstOrDefaultAsync(s => s.Id == board.Id);

            if (board == null && request == null && scholarship == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            ViewData["RequestType"] = request.RequestTypeName;
            ViewData["BoardNo"] = board.BoardNo;
            return View(scholarship);
        }

        [HttpPost]
        [ValidateRole("College Dean", "Vice Dean", "Head of Department")]
        public async Task<IActionResult> RedirectedBoard(int? boardNo)
        {
            if (boardNo == null)
            {
                TempData["Error"] = "Scholarship was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardNo == boardNo);
            var scholarship = await _context.Scholarship.FirstOrDefaultAsync(s => s.Id == board.Id);
            var request = await _context.RequestType.FirstOrDefaultAsync(r => r.RequestTypeID == board.ReqTypeID);
            if (board == null && scholarship == null && request == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            var model = new BoardDetailsViewModel
            {
                Board = board,
                Scholarship = scholarship,
                RequestType = request,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateRole("College Dean", "Vice Dean", "Head of Department")]
        public async Task<IActionResult> SaveRedirectedBoard(BoardDetailsViewModel model)
        {
            if (model.Board != null)
            {
                if (model.Board.BoardNo != 0)
                {
                    model.Board.Reasons += model.Board.AddedReasons;
                    model.Board.AddedReasons = null;
                    if (ModelState.IsValid)
                    {
                        _context.Update(model.Board);
                        _context.Entry(model.Board).Property(b => b.BoardNo).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.BoardSignatures).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.UserSignatures).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.UserRoleSignature).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.ReqDate).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.ReqStatus).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.ReqTypeID).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.DeptMemeberSign1).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.HeadofDeptSign).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.DeptMemeberSign2).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.ViceDeanSign).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.DeanSign).IsModified = false;
                        await _context.SaveChangesAsync();
                        TempData["Success"] = "Board Updated Successfully";
                        var boardDetails = new BoardDetailsViewModel
                        {
                            Id = model.Board.BoardNo,
                        };
                        return RedirectToAction("CollegeBoardSignatures", boardDetails);
                    }
                    else
                    {
                        TempData["Error"] = "Error Occurred when updating Board";
                        return RedirectToAction("CurrentBoards", "Boards");
                    }
                }
                model.Board.ReqDate = DateTime.Now;
                model.Board.Reasons += model.Board.AddedReasons;
                model.Board.AddedReasons = null;
                if (ModelState.IsValid)
                {
                    await _context.Board.AddAsync(model.Board);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Board Saved Successfully";
                    var boardDetails = new BoardDetailsViewModel
                    {
                        Id = model.Board.BoardNo,
                    };
                    return RedirectToAction("CollegeBoardSignatures", boardDetails);
                }
                else
                {
                    TempData["Error"] = "Error Occurred when saving Board";
                    return RedirectToAction("CurrentBoards", "Boards");
                }
            }
            else
            {
                TempData["Error"] = "Error Occurred when saving Board";
                return RedirectToAction("CurrentBoards", "Boards");
            }
        }

        [HttpPost]
        [ValidateRole]
        public async Task<IActionResult> completeBoard(BoardModel model)
        {
            if (model == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardNo == model.BoardNo);
            if (board == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("currentBoards", "Boards");
            }
            if (board.ViceDeanSign && board.DeanSign && board.HeadofDeptSign)
            {
                board.ReqStatus = "completed";
                board.Reasons += model.Recommendation;
                _context.Entry(board).Property(b => b.ReqStatus).IsModified = true;
                _context.Entry(board).Property(b => b.Reasons).IsModified = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "The board has been completed. you can view the board as PDF in Previous Boards Page";
                var vModel = new BoardDetailsViewModel
                {
                    Board = board,
                };
                return RedirectToAction("PreviousBoards", "Boards");
            }
            else
            {
                TempData["Error"] = "Error Occurred when updating Board";
                return RedirectToAction("CurrentBoards", "Boards");
            }
        }

        [ValidateRole]
        public async Task<IActionResult> BoardFile(int? boardNo)
        {
            if (boardNo == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardNo == boardNo);
            if (board == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            var users = new List<UserModel>();
            foreach (var user in board.UserSignatures)
            {
                var userSign = await _userManager.FindByIdAsync(user);
                users.Add(userSign);
            }

            var scholarship = await _context.Scholarship.FirstOrDefaultAsync(s => s.Id == board.Id);
            var modelView = new BoardDetailsViewModel
            {
                Board = board,
                Scholarship = scholarship,
                Users = users,
            };

            var html = await RenderViewAsync("BoardFile", modelView, true);

            var pdf = await GeneratePdfFromHtmlAsync(html);

            return File(pdf, "application/pdf", $"Board_{board.BoardNo}.pdf");
        }

        private async Task<string> RenderViewAsync(string viewName, object model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = ControllerContext.ActionDescriptor.ActionName;
            }

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(ControllerContext, viewName, !partial);

                if (viewEngine is null)
                {
                    throw new InvalidOperationException("ICompositeViewEngine service is not available.");
                }

                if (viewResult.Success == false)
                {
                    throw new ArgumentException($"The view '{viewName}' cannot be found.");
                }

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
        private async Task<byte[]> GeneratePdfFromHtmlAsync(string html)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                Outline = false,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 },
                DocumentTitle = "Board File",
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = html,
                UseLocalLinks = true,
                UseExternalLinks = true,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/lib/bootstrap/dist/css/bootstrap.css"), MinimumFontSize = 18, enablePlugins = true, PrintMediaType = true, LoadImages = true },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings },
            };

            var pdfBytes = _converter.Convert(pdf);

            return pdfBytes;
        }

        [ValidateRole("College Dean", "Vice Dean", "Head of Department")]
        public async Task<IActionResult> CollegeBoardSignatures(BoardDetailsViewModel model)
        {
            var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardNo == model.Id);
            if (board == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            var user = await _userManager.GetUserAsync(User);
            if (board.DeptNo != user.DeptNo)
            {
                TempData["Error"] = "You are not Authorized";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            var dept = await _context.Department.FirstOrDefaultAsync(d => d.DeptNo == board.DeptNo);
            var college = await _context.College.FirstOrDefaultAsync(c => c.CollegeNo == dept.CollegeNo);
            if (college == null && dept == null)
            {
                TempData["Error"] = "Failed to Load College Details";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            ViewData["Department"] = dept.DeptName;
            ViewData["College"] = college.CollegeName;

            return View(board);
        }

        [HttpPost]
        [ValidateRole("Vice Dean")]
        public async Task<IActionResult> ViceDeanSign(int? BoardNo)
        {
            var activeRole = HttpContext.Session.GetString("ActiveRole");

            if(activeRole == null)
            {
                TempData["Error"] = "You are not Authorized";
                return RedirectToAction("CurrentBoards", "Boards");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (BoardNo == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            if (user.Signature == null)
            {
                TempData["Error"] = "You have not uploaded your signature";
                return RedirectToAction("CurrentBoards", "Boards");
            }

            var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardNo == BoardNo);
            if (board == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            if (board.ViceDeanSign == true)
            {
                TempData["Error"] = "You have already signed this board";
                return RedirectToAction("CollegeBoardSignatures");
            }
            if (board.DeptNo != user.DeptNo)
            {
                TempData["Error"] = "You are not Authorized";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            if (board.BoardSignatures == null)
            {
                board.BoardSignatures = new List<string>();
            }
            board.BoardSignatures.Add(user.Signature);
            if (board.UserSignatures == null)
            {
                board.UserSignatures = new List<string>();
            }
            if(board.UserRoleSignature == null)
            {
                board.UserRoleSignature = new List<string>();
            }

            board.UserSignatures.Add(user.Id);
            board.UserRoleSignature.Add(activeRole);
            board.ViceDeanSign = true;
            _context.Update(board);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Board Signed Successfully";
            var vModel = new BoardDetailsViewModel
            {
                Id = board.BoardNo,
            };
            return RedirectToAction("CollegeBoardSignatures", vModel);
        }

        [HttpPost]
        [ValidateRole("College Dean")]
        public async Task<IActionResult> DeanSign(int? BoardNo)
        {
            var activeRole = HttpContext.Session.GetString("ActiveRole");
            if (activeRole == null)
            {
                TempData["Error"] = "You are not Authorized";
                return RedirectToAction("CurrentBoards", "Boards");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (BoardNo == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            if (user.Signature == null)
            {
                TempData["Error"] = "You have not uploaded your signature";
                return RedirectToAction("CurrentBoards", "Boards");
            }

            var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardNo == BoardNo);
            if (board == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            if (board.DeanSign == true)
            {
                TempData["Error"] = "You have already signed this board";
                return RedirectToAction("CollegeBoardSignatures");
            }
            if (board.DeptNo != user.DeptNo)
            {
                TempData["Error"] = "You are not Authorized";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            if (board.BoardSignatures == null)
            {
                board.BoardSignatures = new List<string>();
            }
            board.BoardSignatures.Add(user.Signature);
            if (board.UserSignatures == null)
            {
                board.UserSignatures = new List<string>();
            }
            if(board.UserRoleSignature == null)
            {
                board.UserRoleSignature = new List<string>();
            }

            board.UserSignatures.Add(user.Id);
            board.UserRoleSignature.Add(activeRole);
            board.DeanSign = true;
            _context.Update(board);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Board Signed Successfully";
            var vModel = new BoardDetailsViewModel
            {
                Id = board.BoardNo,
            };
            return RedirectToAction("CollegeBoardSignatures", vModel);
        }

        [HttpPost]
        [ValidateRole("Head of Department")]
        public async Task<IActionResult> HeadofDeptCollegeSign(int? BoardNo)
        {
            var activeRole = HttpContext.Session.GetString("ActiveRole");
            if (activeRole == null)
            {
                TempData["Error"] = "You are not Authorized";
                return RedirectToAction("CurrentBoards", "Boards");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (BoardNo == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            if (user.Signature == null)
            {
                TempData["Error"] = "You have not uploaded your signature";
                return RedirectToAction("CurrentBoards", "Boards");
            }

            var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardNo == BoardNo);
            if (board == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            if (board.HeadofDeptSign == true)
            {
                TempData["Error"] = "You have already signed this board";
                return RedirectToAction("CollegeBoardSignatures");
            }
            if (board.DeptNo != user.DeptNo)
            {
                TempData["Error"] = "You are not Authorized";
                return RedirectToAction("CurrentBoards", "Boards");
            }
            if (board.BoardSignatures == null)
            {
                board.BoardSignatures = new List<string>();
            }
            board.BoardSignatures.Add(user.Signature);
            if (board.UserSignatures == null)
            {
                board.UserSignatures = new List<string>();
            }
            if (board.UserRoleSignature == null)
            {
                board.UserRoleSignature = new List<string>();
            }

            board.UserSignatures.Add(user.Id);
            board.UserRoleSignature.Add(activeRole);
            board.HeadofDeptSign = true;
            _context.Update(board);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Board Signed Successfully";
            var vModel = new BoardDetailsViewModel
            {
                Id = board.BoardNo,
            };
            return RedirectToAction("CollegeBoardSignatures", vModel);
        }


        [HttpPost]
        [ValidateRole("Head of Department", "Department Member")]
        public async Task<IActionResult> SaveBoards(BoardDetailsViewModel model)
        {
            if (model.Board != null)
            {
                if (model.Board.BoardNo != 0)
                {
                    model.Board.Reasons += model.Board.AddedReasons;
                    model.Board.AddedReasons = null;
                    if (ModelState.IsValid)
                    {
                        _context.Update(model.Board);
                        _context.Entry(model.Board).Property(b => b.BoardNo).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.BoardSignatures).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.UserSignatures).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.ReqDate).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.ReqStatus).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.ReqTypeID).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.DeptMemeberSign1).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.HeadofDeptSign).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.DeptMemeberSign2).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.ViceDeanSign).IsModified = false;
                        _context.Entry(model.Board).Property(b => b.DeanSign).IsModified = false;
                        await _context.SaveChangesAsync();
                        TempData["Success"] = "Board Updated Successfully";
                        var boardDetails = new BoardDetailsViewModel
                        {
                            Id = model.Board.BoardNo,
                        };
                        return RedirectToAction("BoardSignatures", boardDetails);
                    }
                    else
                    {
                        TempData["Error"] = "Error Occurred when updating Board";
                        return RedirectToAction("NewScholarshipRequest", "Scholarship");
                    }
                }
                model.Board.ReqDate = DateTime.Now;
                model.Board.Reasons += model.Board.AddedReasons;
                model.Board.AddedReasons = null;
                if (ModelState.IsValid)
                {
                    model.Board.ReqStatus = "department";
                    await _context.Board.AddAsync(model.Board);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Board Saved Successfully";
                    var boardDetails = new BoardDetailsViewModel
                    {
                        Id = model.Board.BoardNo,
                    };
                    return RedirectToAction("BoardSignatures", boardDetails);
                }
                else
                {
                    TempData["Error"] = "Error Occurred when saving Board";
                    return RedirectToAction("NewScholarshipRequest", "Scholarship");
                }
            }
            else
            {
                TempData["Error"] = "Error Occurred when saving Board";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
        }

        [ValidateRole("Head of Department", "Department Member")]
        public async Task<IActionResult> BoardSignatures(BoardDetailsViewModel model)
        {
            var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardNo == model.Id);
            if (board == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var user = await _userManager.GetUserAsync(User);
            if (board.DeptNo != user.DeptNo)
            {
                TempData["Error"] = "You are not Authorized";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var dept = await _context.Department.FirstOrDefaultAsync(d => d.DeptNo == board.DeptNo);
            ViewData["Department"] = dept.DeptName;

            return View(board);
        }

        [HttpPost]
        [ValidateRole("Head of Department")]
        public async Task<IActionResult> HeadDeptSign(int? BoardNo)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (BoardNo == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            if (user.Signature == null)
            {
                TempData["Error"] = "You have not uploaded your signature";
                return RedirectToAction("Index", "Profile");
            }

            var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardNo == BoardNo);
            if (board == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            if (board.HeadofDeptSign == true)
            {
                TempData["Error"] = "You have already signed this board";
                return RedirectToAction("BoardSignatures");
            }
            if (board.DeptNo != user.DeptNo)
            {
                TempData["Error"] = "You are not Authorized";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            if (board.BoardSignatures == null)
            {
                board.BoardSignatures = new List<string>();
            }
            board.BoardSignatures.Add(user.Signature);
            if (board.UserSignatures == null)
            {
                board.UserSignatures = new List<string>();
            }

            board.UserSignatures.Add(user.Id);
            board.HeadofDeptSign = true;
            _context.Update(board);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Board Signed Successfully";
            var vModel = new BoardDetailsViewModel
            {
                Id = board.BoardNo,
            };
            return RedirectToAction("BoardSignatures", vModel);
        }

        [HttpPost]
        [ValidateRole("Department Member")]
        public async Task<IActionResult> DeptMemberSign(int? BoardNo)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (BoardNo == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            if (user.Signature == null)
            {
                TempData["Error"] = "You have not uploaded your signature";
                return RedirectToAction("Index", "Profile");
            }

            var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardNo == BoardNo);
            if (board == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var vModel = new BoardDetailsViewModel
            {
                Id = board.BoardNo,
            };
            if (board.DeptMemeberSign1 == true && board.DeptMemeberSign2 == true)
            {
                TempData["Error"] = "You have already signed this board";
                return RedirectToAction("BoardSignatures", vModel);
            }
            if (board.DeptNo != user.DeptNo)
            {
                TempData["Error"] = "You are not Authorized";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            if (board.BoardSignatures == null)
            {
                board.BoardSignatures = new List<string>();
            }
            board.BoardSignatures.Add(user.Signature);
            if (board.UserSignatures == null)
            {
                board.UserSignatures = new List<string>();
            }
            if (board.UserSignatures != null && board.UserSignatures.Contains(user.Id))
            {
                TempData["Error"] = "You have already signed this board";
                return RedirectToAction("BoardSignatures", vModel);
            }
            board.BoardSignatures.Add(user.Signature);
            board.UserSignatures.Add(user.Id);
            if (board.DeptMemeberSign1 == false)
            {
                board.DeptMemeberSign1 = true;
            }
            else
            {
                board.DeptMemeberSign2 = true;
            }
            _context.Update(board);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Board Signed Successfully";
            return RedirectToAction("BoardSignatures", vModel);
        }

        [HttpPost]
        [ValidateRole("Head of Department")]
        public async Task<IActionResult> BoardSignatures(BoardModel model)
        {
            if (model == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            var board = await _context.Board.FirstOrDefaultAsync(b => b.BoardNo == model.BoardNo);
            if (board == null)
            {
                TempData["Error"] = "Board was not found";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
            if (board.HeadofDeptSign && board.DeptMemeberSign1 && board.DeptMemeberSign2)
            {
                board.ReqStatus = "college";
                board.Reasons += board.Recommendation;
                board.HeadofDeptSign = false;
                board.UserSignatures = null;
                board.UserRoleSignature = null;
                board.BoardSignatures = null;
                _context.Entry(board).Property(b => b.ReqStatus).IsModified = true;
                _context.Entry(board).Property(b => b.HeadofDeptSign).IsModified = true;
                _context.Entry(board).Property(b => b.Reasons).IsModified = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "The board has been completed and successfully sent to the Vice Dean. Great Job 👍";
                var vModel = new BoardDetailsViewModel
                {
                    Id = board.BoardNo,
                };
                return RedirectToAction("CurrentBoards", "Boards");
            }
            else
            {
                TempData["Error"] = "Error Occurred when updating Board";
                return RedirectToAction("NewScholarshipRequest", "Scholarship");
            }
        }
    }
}