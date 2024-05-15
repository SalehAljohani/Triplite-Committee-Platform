using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateRole("Admin")]
    public class AnnouncementController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<AnnouncementController> Localizer;


        public AnnouncementController(AppDbContext context, IStringLocalizer<AnnouncementController> localizer)
        {
            _context = context;
            Localizer = localizer;
        }
        public IActionResult Index()
        {
            var announcements = _context.Announcements.ToList();

            var AnnonList = new List<AnnouncementModel>();
            foreach (var item in announcements)
            {
                var model = new AnnouncementModel();
                var department = _context.Department.Where(d => d.DeptNo == item.DeptNo).FirstOrDefault();
                model.Department = department;
                model.Title = item.Title;
                model.Description = item.Description;
                model.Link = item.Link;
                model.DeptNo = item.DeptNo;
                model.Id = item.Id;
                AnnonList.Add(model);
            }

            return View(AnnonList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                string announcNotFound = @Localizer["announcNotFound"];
                TempData["Message"] = announcNotFound;
                return RedirectToAction(nameof(Index));
            }

            var announcementModel = _context.Announcements.FirstOrDefault(m => m.Id == id);

            if (announcementModel == null)
            {
                string announcNotFound = @Localizer["announcNotFound"];
                TempData["Message"] = announcNotFound;
                return RedirectToAction(nameof(Index));
            }

            var dept = await _context.Department.Where(d => d.DeptNo == announcementModel.DeptNo).FirstOrDefaultAsync();

            if (dept == null)
            {
                string deptNotFound = @Localizer["deptNotFound"];
                TempData["Message"] = deptNotFound;
                return RedirectToAction(nameof(Index));
            }

            var college = await _context.College.Where(c => c.CollegeNo == dept.CollegeNo).FirstOrDefaultAsync();
            if (college == null)
            {
                string collegeNotFound = @Localizer["collegeNotFound"];
                TempData["Message"] = collegeNotFound;
                return RedirectToAction(nameof(Index));
            }

            ViewData["College"] = college.CollegeName;

            announcementModel.Department = dept;

            return View(announcementModel);
        }

        public IActionResult Create()
        {
            var college = _context.College.ToList();
            ViewData["Colleges"] = new SelectList(college, "CollegeNo", "CollegeName");

            var department = _context.Department.ToList();
            var departmentsData = department.Select(d => new
            {
                DeptNo = d.DeptNo,
                DeptName = d.DeptName,
                CollegeNo = d.CollegeNo
            }).ToList();

            ViewData["Departments"] = Newtonsoft.Json.JsonConvert.SerializeObject(departmentsData);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AnnouncementModel announcement)
        {
            if (ModelState.IsValid)
            {
                _context.Announcements.Add(announcement);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(announcement);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                string announcNotFound = @Localizer["announcNotFound"];
                TempData["Message"] = announcNotFound;
                return RedirectToAction(nameof(Index));
            }

            var announcementModel = await _context.Announcements.FindAsync(id);
            if (announcementModel == null)
            {
                string announcNotFound = @Localizer["announcNotFound"];
                TempData["Message"] = announcNotFound;
                return RedirectToAction(nameof(Index));
            }

            var department = await _context.Department.Where(d => d.DeptNo == announcementModel.DeptNo).FirstOrDefaultAsync();
            if(department == null)
            {
                string errorLoadDept = @Localizer["errorLoadDept"];
                TempData["Message"] = errorLoadDept;
                return RedirectToAction(nameof(Index));
            }

            var college = await _context.College.Where(c => c.CollegeNo == department.CollegeNo).FirstOrDefaultAsync();
            if(college == null)
            {
                string errorLoadCollege = @Localizer["errorLoadCollege"];
                TempData["Message"] = errorLoadCollege;
                return RedirectToAction(nameof(Index));
            }

            ViewData["SelectedDept"] = department;
            ViewData["SelectedCollege"] = college;

            var colleges = await _context.College.ToListAsync();
            ViewData["Colleges"] = new SelectList(colleges, "CollegeNo", "CollegeName", college);

            var departments = await _context.Department.ToListAsync();
            var departmentsData = departments.Select(d => new
            {
                DeptNo = d.DeptNo,
                DeptName = d.DeptName,
                CollegeNo = d.CollegeNo
            }).ToList();
            ViewData["Departments"] = Newtonsoft.Json.JsonConvert.SerializeObject(departmentsData);

            return View(announcementModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AnnouncementModel announcementModel)
        {
            if (id != announcementModel.Id)
            {
                string announcNotFound = @Localizer["announcNotFound"];
                TempData["Message"] = announcNotFound;
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var existingAnnouncement = await _context.Announcements.FindAsync(id);
                if (existingAnnouncement == null)
                {
                    string announcNotFound = @Localizer["announcNotFound"];
                    TempData["Message"] = announcNotFound;
                    return RedirectToAction(nameof(Index));
                }

                if (announcementModel.DeptNo == 0)
                {
                    announcementModel.DeptNo = existingAnnouncement.DeptNo;
                }

                try
                {
                    _context.Entry(existingAnnouncement).CurrentValues.SetValues(announcementModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementModelExists(announcementModel.Id))
                    {
                        string announcNotFound = @Localizer["announcNotFound"];
                        TempData["Message"] = announcNotFound;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(announcementModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                string announcNotFound = @Localizer["announcNotFound"];
                TempData["Message"] = announcNotFound;
                return RedirectToAction(nameof(Index));
            }

            var announcementModel = await _context.Announcements.FirstOrDefaultAsync(m => m.Id == id);
            if (announcementModel == null)
            {
                string announcNotFound = @Localizer["announcNotFound"];
                TempData["Message"] = announcNotFound;
                return RedirectToAction(nameof(Index));
            }

            var dept = await _context.Department.Where(d => d.DeptNo == announcementModel.DeptNo).FirstOrDefaultAsync();
            if (dept == null)
            {
                string deptNotFound = @Localizer["deptNotFound"];
                TempData["Message"] = deptNotFound;
                return RedirectToAction(nameof(Index));
            }

            var college = await _context.College.Where(c => c.CollegeNo == dept.CollegeNo).FirstOrDefaultAsync();
            if (college == null)
            {
                string collegeNotFound = @Localizer["collegeNotFound"];
                TempData["Message"] = collegeNotFound;
                return RedirectToAction(nameof(Index));
            }

            ViewData["College"] = college.CollegeName;

            announcementModel.Department = dept;

            return View(announcementModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcementModel = await _context.Announcements.FindAsync(id);
            _context.Announcements.Remove(announcementModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnnouncementModelExists(int id)
        {
            return _context.Announcements.Any(e => e.Id == id);
        }
    }
}
