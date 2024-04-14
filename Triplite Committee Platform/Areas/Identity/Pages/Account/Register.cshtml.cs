
////using System;
////using System.Collections.Generic;
////using System.ComponentModel.DataAnnotations;
////using System.Linq;
////using System.Text;
////using System.Text.Encodings.Web;
////using System.Threading;
////using System.Threading.Tasks;
////using Microsoft.AspNetCore.Authentication;
////using Microsoft.AspNetCore.Authorization;
////using Microsoft.AspNetCore.Identity;
////using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.AspNetCore.Mvc;
////using Microsoft.AspNetCore.Mvc.ApplicationModels;
//using Microsoft.AspNetCore.Mvc.RazorPages;
////using Microsoft.AspNetCore.WebUtilities;
////using Microsoft.EntityFrameworkCore;
////using Microsoft.Extensions.Logging;
////using Triplite_Committee_Platform.Data;
////using Triplite_Committee_Platform.Models;

//namespace Triplite_Committee_Platform.Areas.Identity.Pages.Account
//{
//    public class RegisterModel : PageModel
//    {
//        public IActionResult OnGet()
//        {
//            return RedirectToPage("/Error"); // Redirect to the error page
//        }

//        public IActionResult OnPost()
//        {
//            return RedirectToPage("/Error");
//        }
//    }
//}
////    Disabling it for now, to be deleted later after confirming UserController is working fine.
////    {
////    public class RegisterModel : PageModel
////    {
////        private readonly IEmailSender _emailSender;
////        private readonly IUserEmailStore<UserModel> _emailStore;
////        private readonly ILogger<RegisterModel> _logger;
////        private readonly SignInManager<UserModel> _signInManager;
////        private readonly UserManager<UserModel> _userManager;
////        private readonly IUserStore<UserModel> _userStore;
////        private readonly AppDbContext _context;
////        private readonly RoleManager<IdentityRole> _roleManager;

////        public RegisterModel(
////            UserManager<UserModel> userManager,
////            IUserStore<UserModel> userStore,
////            SignInManager<UserModel> signInManager,
////            ILogger<RegisterModel> logger,
////            IEmailSender emailSender,
////            AppDbContext context,
////            RoleManager<IdentityRole> roleManager
////            )
////        {
////            _userManager = userManager;
////            _userStore = userStore;
////            _emailStore = GetEmailStore();
////            _signInManager = signInManager;
////            _logger = logger;
////            _emailSender = emailSender;
////            _context = context;
////            _roleManager = roleManager;
////        }

////        public IList<DepartmentModel> Departments { get; set; }
////        public IList<IdentityRole> Roles { get; set; }


////        [BindProperty]
////        public InputModel Input { get; set; }

////        public string ReturnUrl { get; set; }

////        public async Task OnGetAsync(string returnUrl = null)
////        {
////            ReturnUrl = returnUrl;

////            Departments = await _context.Department.ToListAsync();
////            Roles = await _roleManager.Roles.ToListAsync();
////        }

////        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
////        {
////            returnUrl ??= Url.Content("~/");

////            if (ModelState.IsValid)
////            {
////                var user = CreateUser();
////                user.Name = Input.Name;
////                user.DeptNo = Input.DeptNo;
////                user.EmployeeID = Input.EmployeeID;

////                // Set the username to the EmployeeID
////                await _userStore.SetUserNameAsync(user, Input.EmployeeID.ToString(), CancellationToken.None);
////                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
////                var result = await _userManager.CreateAsync(user, Input.Password);

////                if (result.Succeeded)
////                {
////                    if (await _roleManager.RoleExistsAsync(Input.Role))
////                    {
////                        await _userManager.AddToRoleAsync(user, Input.Role);

////                        _logger.LogInformation("User created a new account with password.");

////                        var userId = await _userManager.GetUserIdAsync(user);
////                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

////                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
////                        var callbackUrl = Url.Page(
////                            "/Account/ConfirmEmail",
////                            pageHandler: null,
////                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
////                            protocol: Request.Scheme);

////                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
////                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

////                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
////                        {
////                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
////                        }
////                        else
////                        {
////                            await _signInManager.SignInAsync(user, isPersistent: false);
////                            return LocalRedirect(returnUrl);
////                        }
////                    }
////                    else
////                    {
////                        ModelState.AddModelError(string.Empty, "The selected role does not exist.");
////                    }
////                }
////                foreach (var error in result.Errors)
////                {
////                    ModelState.AddModelError(string.Empty, error.Description);
////                }
////            }

////            // If we got this far, something failed, redisplay form
////            Departments = await _context.Department.ToListAsync();
////            Roles = await _roleManager.Roles.ToListAsync();
////            return Page();
////        }

////        private UserModel CreateUser()
////        {
////            try
////            {
////                return Activator.CreateInstance<UserModel>();
////            }
////            catch
////            {
////                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserModel)}'. " +
////                    $"Ensure that '{nameof(UserModel)}' is not an abstract class and has a parameterless constructor, or alternatively " +
////                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
////            }
////        }

////        private IUserEmailStore<UserModel> GetEmailStore()
////        {
////            if (!_userManager.SupportsUserEmail)
////            {
////                throw new NotSupportedException("The default UI requires a user store with email support.");
////            }
////            return (IUserEmailStore<UserModel>)_userStore;
////        }

////        public class InputModel
////        {
////            [DataType(DataType.Password)]
////            [Display(Name = "Confirm password")]
////            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
////            public string ConfirmPassword { get; set; }

////            [Required]
////            [EmailAddress]
////            [RegularExpression(@"^[a-zA-Z0-9._%+-]+@taibahu\.edu\.sa$", ErrorMessage = "Please enter an email from the @taibahu.edu.sa domain.")]
////            [Display(Name = "Email")]
////            public string Email { get; set; }

////            [Required]
////            [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
////            [DataType(DataType.Password)]
////            [Display(Name = "Password")]
////            public string Password { get; set; }

////            [Required]
////            [StringLength(35, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
////            [RegularExpression(@"^[a-zA-Z\s\u0621-\u064A]*$", ErrorMessage = "The Name field should only contain letters, spaces.")]
////            [Display(Name = "Name")]
////            public string Name { get; set; }

////            [Required]
////            [Display(Name = "Employee ID")]
////            [RegularExpression("^[0-9]{10}$", ErrorMessage = "Employee ID must be a 10 numbers")]
////            public int EmployeeID { get; set; }

////            [Required(ErrorMessage = "Must pick a department")]
////            [Display(Name = "Department")]
////            public int DeptNo { get; set; }

////            [Required(ErrorMessage = "Must pick a role")]
////            [Display(Name = "Role")]
////            public string Role { get; set; }
////        }
////    }
////}