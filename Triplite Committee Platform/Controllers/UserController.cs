using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.ViewModels;
using Triplite_Committee_Platform.Services;

namespace Triplite_Committee_Platform.Controllers
{
    public class UserController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IUserEmailStore<UserModel> _emailStore;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly IUserStore<UserModel> _userStore;
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UserController(
            UserManager<UserModel> userManager,
            IUserStore<UserModel> userStore,
            SignInManager<UserModel> signInManager,
            ILogger<UserController> logger,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            IEmailSender emailSender
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();


            foreach (var user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                var roles = await _userManager.GetRolesAsync(user);
                thisViewModel.Id = user.Id;
                thisViewModel.Name = user.Name;
                thisViewModel.EmployeeID = user.EmployeeID;
                thisViewModel.Email = user.Email;
                thisViewModel.EmailConfirmed = user.EmailConfirmed;
                thisViewModel.PhoneNumber = user.PhoneNumber;
                thisViewModel.Department = await _context.Department.FirstOrDefaultAsync(d => d.DeptNo == user.DeptNo);
                thisViewModel.ListRoles = roles;

                userRolesViewModel.Add(thisViewModel);
            }

            return View(userRolesViewModel);
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.Users.Include(u => u.Department).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(user);

            var thisViewModel = new UserRolesViewModel();
            thisViewModel.Id = user.Id;
            thisViewModel.Name = user.Name;
            thisViewModel.EmployeeID = user.EmployeeID;
            thisViewModel.Email = user.Email;
            thisViewModel.EmailConfirmed = user.EmailConfirmed;
            thisViewModel.PhoneNumber = user.PhoneNumber;
            thisViewModel.Department = await _context.Department.FirstOrDefaultAsync(d => d.DeptNo == user.DeptNo);
            thisViewModel.ListRoles = await _userManager.GetRolesAsync(user);

            return View(thisViewModel);
        }

        // GET: Department/Create
        public async Task<IActionResult> Create()
        {
            var department = await _context.Department.ToListAsync();

            ViewData["Departments"] = new SelectList(department, "DeptNo", "DeptName");
            ViewData["Roles"] = _roleManager.Roles.ToList();
            return View();
        }

        //POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRolesViewModel userRolesViewModel, List<string> ListRoles)
        {
            var department = userRolesViewModel.Department;
            if (ModelState.IsValid)
            {
                var user = new UserRolesViewModel
                {
                    Name = userRolesViewModel.Name,
                    EmployeeID = userRolesViewModel.EmployeeID,
                    PhoneNumber = userRolesViewModel.PhoneNumber,
                    DeptNo = userRolesViewModel.DeptNo
                };
                if(userRolesViewModel.DeptNo != null)
                {
                    user.Department = await _context.Department.FirstOrDefaultAsync(d => d.DeptNo == userRolesViewModel.DeptNo);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Please Select a Department.");
                    await PopulateDepartmentsAndRoles();
                    return View(userRolesViewModel);
                }
                if (ListRoles == null)
                {
                    ModelState.AddModelError(string.Empty, "Please Select a Departmentsssssssss.");
                    await PopulateDepartmentsAndRoles();
                    return View(userRolesViewModel);
                }
                if (await _userManager.FindByEmailAsync(userRolesViewModel.Email) != null)
                {
                    ModelState.AddModelError(string.Empty, "Email already exists.");
                    await PopulateDepartmentsAndRoles();
                    return View(userRolesViewModel);
                }
                if (await _userManager.FindByNameAsync(userRolesViewModel.EmployeeID.ToString()) != null)
                {
                    ModelState.AddModelError(string.Empty, "Employee ID already exists.");
                    await PopulateDepartmentsAndRoles();
                    return View(userRolesViewModel);
                }
                if (await _userManager.FindByNameAsync(userRolesViewModel.Name) != null)
                {
                    ModelState.AddModelError(string.Empty, "Name already exists.");
                    await PopulateDepartmentsAndRoles();
                    return View(userRolesViewModel);
                }
                await _userStore.SetUserNameAsync(user, userRolesViewModel.EmployeeID.ToString(), CancellationToken.None);
                await _emailStore.SetEmailAsync(user, userRolesViewModel.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, userRolesViewModel.Password);
                if (result.Succeeded)
                {
                    if (ListRoles.Count > 0)
                    {
                        if (ListRoles == null)
                        {
                            ModelState.AddModelError(string.Empty, "Please select at least one role.");
                            await PopulateDepartmentsAndRoles();
                            return View(userRolesViewModel);
                        }
                        foreach (var role in ListRoles)
                        {
                            if (await _roleManager.RoleExistsAsync(role))
                            {
                                await _userManager.AddToRoleAsync(user, role);
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Role does not exist.");
                                await PopulateDepartmentsAndRoles();
                                return View(userRolesViewModel);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Please select at least one role.");
                        await PopulateDepartmentsAndRoles();
                        return View(userRolesViewModel);
                    }

                    // Send email confirmation
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedToken = Encoding.UTF8.GetBytes(token);
                    var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = validToken }, Request.Scheme);
                    await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    TempData["Success"] = "User created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            await PopulateDepartmentsAndRoles();
            TempData["Error"] = "User creation failed.";
            return View(userRolesViewModel);
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userRolesViewModel = new UserRolesViewModel
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                EmployeeID = user.EmployeeID,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Department = user.Department,
                ListRoles = roles
            };
            if(userRolesViewModel.PhoneNumber == null)
            {
                userRolesViewModel.PhoneNumber = "No Phone Number";
            }
            else
            {
                userRolesViewModel.PhoneNumber = user.PhoneNumber;
            }
            await PopulateDepartmentsAndRoles();
            return View(userRolesViewModel);
        }

        //POST: Department/Edit/5 review the code
       [HttpPost]
       [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserRolesViewModel userRolesViewModel)
        {
            if (id != userRolesViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    if (userRolesViewModel.Name != user.Name)
                    {
                        if (userRolesViewModel.Name != null)
                        {
                            user.Name = userRolesViewModel.Name;
                        }
                    }
                    if(userRolesViewModel.PhoneNumber != user.PhoneNumber)
                    {
                        if (userRolesViewModel.PhoneNumber != null)
                        {
                            user.PhoneNumber = userRolesViewModel.PhoneNumber;
                        }
                    }
                    if(userRolesViewModel.Department != user.Department)
                    {
                           if (userRolesViewModel.Department != null)
                        {
                            user.Department = userRolesViewModel.Department;
                        }
                    }
                    var roles = await _userManager.GetRolesAsync(user);

                    if (userRolesViewModel.ListRoles != roles)
                    {
                        if(userRolesViewModel.ListRoles != null)
                        {
                            await _userManager.RemoveFromRolesAsync(user, roles);
                            foreach (var role in userRolesViewModel.ListRoles)
                            {
                                await _userManager.AddToRoleAsync(user, role);
                            }
                        }
                    }
                    await _userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userRolesViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form

            await PopulateDepartmentsAndRoles();
            return View(userRolesViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, UserRolesViewModel userRolesViewModel)
        {
            if (id != userRolesViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, userRolesViewModel.Password);
                    if (result.Succeeded)
                    {
                        TempData["PasswordSuccess"] = "Password changed successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    TempData["PasswordError"] = "Password change failed.";
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userRolesViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            await PopulateDepartmentsAndRoles();
            return View(userRolesViewModel);
        }

        // GET: Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
        // POST: Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }


        private async Task PopulateDepartmentsAndRoles()
        {
            var department = await _context.Department.ToListAsync();
            ViewData["Departments"] = new SelectList(department, "DeptNo", "DeptName");
            ViewData["Roles"] = _roleManager.Roles.ToList();
        }

        private UserModel CreateUser()
        {
            try
            {
                return Activator.CreateInstance<UserModel>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserModel)}'. " +
                    $"Ensure that '{nameof(UserModel)}' is not an abstract class and has a parameterless constructor");
            }
        }

        private IUserEmailStore<UserModel> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<UserModel>)_userStore;
        }

        // UserExists check if user exists simply :)
        // using userManager which is method of IdentityUser 
        private bool UserExists(string id)
        {
            return _userManager.Users.Any(u => u.Id == id);
        }
    }
}
