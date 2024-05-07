using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;
using Triplite_Committee_Platform.ViewModels;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    [ValidateRole]
    public class ProfileController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        public ProfileController(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
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

            return View(user);
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
                    TempData["Message"] = "Old password is incorrect.";
                    return RedirectToAction(nameof(Index));
                }

                var result = await _userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);
                if (result.Succeeded)
                {
                    TempData["Message"] = "Password updated successfully.";
                }
                else
                {
                    TempData["Message"] = "An error occurred while updating your password.";
                }
            }
            else
            {
                TempData["Message"] = "An error occurred while updating your password.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSign(string? Image)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }



            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "Profile updated successfully.";
            }
            else
            {
                TempData["Message"] = "An error occurred while updating your profile.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }


            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "Profile updated successfully.";
            }
            else
            {
                TempData["Message"] = "An error occurred while updating your profile.";
            }

            return RedirectToAction("Index");
        }
    }
}
