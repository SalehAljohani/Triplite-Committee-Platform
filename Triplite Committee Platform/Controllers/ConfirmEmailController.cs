using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    public class ConfirmEmailController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _userSign;
        private readonly EmailSender _emailSender;

        public ConfirmEmailController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, EmailSender emailSender)
        {
            _userManager = userManager;
            _userSign = signInManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user.EmailConfirmed == false)
            {
                reSendEmail(user);
                ViewData["ConfirmEmail"] = "Must Verify Your Account First, Please check your email.";
                return View();
            }
            else 
            { 
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                ModelState.AddModelError(string.Empty, "Verification Failed.");
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                await _userSign.SignInAsync(user, isPersistent: false);
                TempData["ConfirmEmail"] = "Thank you for confirming your email.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        private async void reSendEmail(UserModel user)
        {
            if (user.Email != null)
            {
                if (user.EmailConfirmed == false)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedToken = Encoding.UTF8.GetBytes(token);
                    var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                    var callbackUrl = Url.Action("ConfirmEmail", "ConfirmEmail", new { userId = user.Id, token = validToken }, Request.Scheme);
                    var dynamicTemplateData = new { Subject = "Account Confirmation", ConfirmLink = callbackUrl };
                    var templateId = "d-667c9f91699d4836b1ba1aab66bb0295";
                    await _emailSender.SendEmailAsync(user.Email, templateId, dynamicTemplateData);
                }
            }
        }
    }
}
