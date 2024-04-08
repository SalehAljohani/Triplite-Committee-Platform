using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    public class ConfirmEmailController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly EmailSender _emailSender;

        public ConfirmEmailController(UserManager<UserModel> userManager, EmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                RedirectToAction("Index", "Home");
            }
            if (user.EmailConfirmed == false)
            {
                ViewData["ConfirmEmail"] = "Must verify your account first, Please check your email.";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmail()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                RedirectToAction("Index", "Home");
            }
            if (user.Email != null)
            {
                if (user.EmailConfirmed == false)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedToken = Encoding.UTF8.GetBytes(token);
                    var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                    var callbackUrl = Url.Action("Index", "SetPassword", new { userId = user.Id, token = validToken }, Request.Scheme);
                    var dynamicTemplateData = new { Subject = "Account Confirmation", ConfirmLink = callbackUrl };
                    var templateId = "d-ca5e1fdee08047d1afa4448fe1cee09a";
                    await _emailSender.SendEmailAsync(user.Email, templateId, dynamicTemplateData);

                    TempData["EmailSent"] = "Verification Email Sent ✔";
                    return RedirectToAction("Index");
                }
            }
            return View("Index");
        }
    }
}
