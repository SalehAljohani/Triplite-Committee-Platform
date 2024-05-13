using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public ProfileController(UserManager<UserModel> userManager, AppDbContext context, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _context = context;
            _env = env;
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
                TempData["Message"] = "You need to confirm your email before proceeding.";
                return RedirectToAction("Index", "ConfirmEmail");
            }
            var activeRole = HttpContext.Session.GetString("ActiveRole");
            var userDept = await _context.Department.FirstOrDefaultAsync(x => x.DeptNo == user.DeptNo);
            if (userDept == null)
            {
                TempData["Error"] = "An error occurred while fetching your profile.";
                return RedirectToAction(nameof(Index));
            }
            var userCollege = await _context.College.FirstOrDefaultAsync(x => x.CollegeNo == userDept.CollegeNo);
            if (userCollege == null)
            {
                TempData["Error"] = "An error occurred while fetching your profile.";
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
                    TempData["Error"] = "Old password is incorrect.";
                    return RedirectToAction(nameof(Index));
                }

                var result = await _userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);
                if (result.Succeeded)
                {
                    TempData["Message"] = "Password updated successfully.";
                }
                else
                {
                    TempData["Error"] = "An error occurred while updating your password.";
                }
            }
            else
            {

                TempData["Error"] = "An error occurred while updating your password.";
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
                TempData["Error"] = "You havent Draw or Upload Signature";
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
                    TempData["Error"] = "Invalid file type. Only image files are allowed.";
                    return RedirectToAction("Index");
                }

                if (Path.GetExtension(file.FileName).ToLower() != ".png")
                {
                    TempData["Error"] = "Invalid file type. Only PNG files are allowed.";
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
                TempData["Message"] = "Signature was added successfully.";
            }
            else
            {
                TempData["Message"] = "An error occurred while uploading your Signature.";
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
                TempData["Message"] = "Signature was removed successfully.";
            }
            else
            {
                TempData["Message"] = "An error occurred while removing your Signature.";
            }

            return RedirectToAction("Index");
        }
    }
}
