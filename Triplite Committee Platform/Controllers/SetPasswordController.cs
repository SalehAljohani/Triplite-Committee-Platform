using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using System.Text;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.ViewModels;

namespace Triplite_Committee_Platform.Controllers
{
    public class SetPasswordController : BaseController
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IStringLocalizer<SetPasswordController> Localizer;
        public SetPasswordController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IStringLocalizer<SetPasswordController> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Localizer = localizer;
        }

        public async Task<IActionResult> Index(string userId, string token)
        {
            if (userId == null || token == null)
            {
                string verifyFail = @Localizer["verifyFail"];
                ModelState.AddModelError(string.Empty, verifyFail);
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                string verifyFail = @Localizer["verifyFail"];
                TempData["Error"] = verifyFail;
                return RedirectToAction("Index", "Home");
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                string thanks = @Localizer["thanks"];
                TempData["ConfirmEmail"] = thanks;
                return RedirectToAction("SetPassword", "SetPassword");
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> SetPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                string userNotFound = @Localizer["userNotFound"];
                TempData["Error"] = userNotFound;
                return RedirectToAction("Index", "Home");
            }
            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (hasPassword)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                string userNotFound = @Localizer["userNotFound"];
                TempData["Error"] = userNotFound;
                return RedirectToAction("Index", "Home");
            }
            
            var result = await _userManager.AddPasswordAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Index", "Home");
            }

            return View(model);

        }
    }
}
