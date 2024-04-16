using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;
using Triplite_Committee_Platform.ViewModels;

namespace Triplite_Committee_Platform.Controllers
{

    public class LoginController : Controller
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly EmailSender _emailSender;
        private readonly AppDbContext _context;
        public LoginController(SignInManager<UserModel> signInManager, UserManager<UserModel> userManager, EmailSender emailSender, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SetPassword", "SetPassword");
            }
            return View();
        }

        [HttpPost, ActionName("Index")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = new EmailAddressAttribute().IsValid(model.Email) ? await _userManager.FindByEmailAsync(model.Email) : null;
            var username = user != null ? user.UserName : model.Email;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(username, model.Password!, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("SetPassword", "SetPassword");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SetPassword", "SetPassword");
            }
            return View();
        }

        // POST: ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (Email != null)
            {
                if (new EmailAddressAttribute().IsValid(Email))
                {
                    var user = await _userManager.FindByEmailAsync(Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        // For Security Perposes, we don't want to reveal that the user does not exist or is not confirmed
                        return RedirectToAction("ForgotPasswordConfirmation");
                    }

                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Action("ResetPassword", "Login", values: new { code }, protocol: Request.Scheme);
                    var dynamicTemplateData = new { Subject = "Password Reset", ResetPassword = callbackUrl };
                    var templateId = "d-76703abddcd74314b76d4e7d0b819313";
                    await _emailSender.SendEmailAsync(user.Email, templateId, dynamicTemplateData);
                    return RedirectToAction("ForgotPasswordConfirmation");
                }
            }

            return View();
        }


        public IActionResult ForgotPasswordConfirmation()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SetPassword", "SetPassword");
            }
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                TempData["Error"] = "A code must be supplied for password reset.";
                return RedirectToAction("Index", "Login");
            }
            var model = new ResetPasswordViewModel { Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)) };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Index", "Login");
            }
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var result = await _userManager.ResetPasswordAsync(user, code, model.Password);
            if (result.Succeeded)
            {
                TempData["Success"] = "Password reset successful.";
                return RedirectToAction("Index", "Login");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        public IActionResult Support()
        {
            ViewData["SupportDetails"] = _context.SupportPage.FirstOrDefault();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Support(ContactModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _context.Contact.AddAsync(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Message sent successfully.";
            TempData["SuccessMessage"] = "Thank you for contacting us. We will get back to you soon.";

            ModelState.Clear();
            ViewData["SupportDetails"] = _context.SupportPage.FirstOrDefault();
            return View("Support");
        }

        public IActionResult Help()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["Logout"] = "You have been logged out.";
            return RedirectToAction("Index", "Login");
        }
    }
}
