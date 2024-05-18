using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;
using Triplite_Committee_Platform.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    [ValidateRole]
    public class ProfileController : BaseController
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IStringLocalizer<ProfileController> Localizer;
        public ProfileController(UserManager<UserModel> userManager, AppDbContext context, IWebHostEnvironment env, IStringLocalizer<ProfileController>localizer)
        {
            _userManager = userManager;
            _context = context;
            _env = env;
            Localizer = localizer;
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
                string confirmEmail = @Localizer["confirmEmail"];
                TempData["Message"] = confirmEmail;
                return RedirectToAction("Index", "ConfirmEmail");
            }
            var activeRole = HttpContext.Session.GetString("ActiveRole");
            var userDept = await _context.Department.FirstOrDefaultAsync(x => x.DeptNo == user.DeptNo);
            if (userDept == null)
            {
                string errorFetch = @Localizer["errorFetch"];
                TempData["Error"] = errorFetch;
                return RedirectToAction(nameof(Index));
            }
            var userCollege = await _context.College.FirstOrDefaultAsync(x => x.CollegeNo == userDept.CollegeNo);
            if (userCollege == null)
            {
                string errorFetch = @Localizer["errorFetch"];
                TempData["Error"] = errorFetch;
                return RedirectToAction(nameof(Index));
            }
            var model = new ProfileViewModel
            {
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmployeeID = user.EmployeeID,
                College = userCollege,
                Department = userDept,
                activeRole = activeRole,
                Signature = user.Signature
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                if (!await _userManager.CheckPasswordAsync(user, model.oldPassword))
                {
                    string oldPass = @Localizer["oldPass"];
                    TempData["Error"] = oldPass;
                    return RedirectToAction(nameof(Index));
                }

                var result = await _userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);
                if (result.Succeeded)
                {
                    string passUpdate = @Localizer["passUpdate"];
                    TempData["Message"] = passUpdate;
                }
                else
                {
                    string errorUpdate = @Localizer["errorUpdate"];
                    TempData["Error"] = errorUpdate;
                }
            }
            else
            {
                string errorUpdate = @Localizer["errorUpdate"];
                TempData["Error"] = errorUpdate;
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSign(string? signatureData, IFormFile? file)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if(string.IsNullOrEmpty(signatureData) && file == null)
            {
                string draw = @Localizer["draw"];
                TempData["Error"] = draw;
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(signatureData))
            {
                var base64Signature = signatureData?.Split(',')[1];
                var signatureBytes = Convert.FromBase64String(base64Signature);

                var fileName = $"{user.UserName}_Sign.png";

                var filePath = Path.Combine(_env.WebRootPath, "signatures", fileName);
                await System.IO.File.WriteAllBytesAsync(filePath, signatureBytes);

                user.Signature = fileName;
            }

            if(file != null)
            {
                if (!file.ContentType.StartsWith("image/"))
                {
                    string invalidImage = @Localizer["invalidImage"];
                    TempData["Error"] = invalidImage;
                    return RedirectToAction("Index");
                }

                if (Path.GetExtension(file.FileName).ToLower() != ".png")
                {
                    string invalidPNG = @Localizer["invalidPNG"];
                    TempData["Error"] = invalidPNG;
                    return RedirectToAction("Index");
                }

                var fileName = $"{user.UserName}_Sign.png";
                var filePath = Path.Combine(_env.WebRootPath, "signatures", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                user.Signature = fileName;
            }
            
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                string signAdd = @Localizer["signAdd"];
                TempData["Message"] = signAdd;
            }
            else
            {
                string errorSignUpload = @Localizer["errorSignUpload"];
                TempData["Message"] = errorSignUpload;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSign()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var signatureFilePath = Path.Combine(_env.WebRootPath, "signatures", user.Signature);

            if (System.IO.File.Exists(signatureFilePath))
            {
                System.IO.File.Delete(signatureFilePath);
            }

            user.Signature = null;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                string signRemove = @Localizer["signRemove"];
                TempData["Message"] = signRemove;
            }
            else
            {
                string errorSignRemove = @Localizer["errorSignRemove"];
                TempData["Message"] = errorSignRemove;
            }

            return RedirectToAction("Index");
        }
    }
}
