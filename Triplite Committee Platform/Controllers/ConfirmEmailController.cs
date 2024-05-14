using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using System.Text;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    public class ConfirmEmailController : BaseController
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly EmailSender _emailSender;
        private readonly IStringLocalizer<ConfirmEmailController> Localizer ;

        public ConfirmEmailController(UserManager<UserModel> userManager, EmailSender emailSender, IStringLocalizer<ConfirmEmailController> localizer)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            Localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {

                if (user.EmailConfirmed == false)
                {
                    ViewData["ConfirmEmail"] = @Localizer["verifyAcc"];
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmail(string? Email)
        {
            if(Email == null)
            {
                return View("Index");
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return View("Index");
            }
            else
            {
                if (user.Email != null)
                {
                    if (user.EmailConfirmed == false)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var encodedToken = Encoding.UTF8.GetBytes(token);
                        var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                        var callbackUrl = Url.Action("Index", "SetPassword", new { userId = user.Id, token = validToken }, Request.Scheme);
                        var dynamicTemplateData = new { Subject = @Localizer["accConfirm"], ConfirmLink = callbackUrl };
                        var templateId = "d-ca5e1fdee08047d1afa4448fe1cee09a";
                        await _emailSender.SendEmailAsync(user.Email, templateId, dynamicTemplateData);

                        TempData["EmailSent"] = @Localizer["verifySent"];
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index", "Login");
                }
                return View("Index");
            }
        }
    }
}
