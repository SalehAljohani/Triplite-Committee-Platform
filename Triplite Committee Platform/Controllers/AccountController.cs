using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Text;
using Triplite_Committee_Platform.Data;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.Services;
using Triplite_Committee_Platform.ViewModels;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateRole("Admin")]
    public class AccountController : BaseController
    {
        private readonly EmailSender _emailSender;
        private readonly IUserEmailStore<UserModel> _emailStore;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly IUserStore<UserModel> _userStore;
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStringLocalizer<AccountController> Localizer;


        public AccountController(
            UserManager<UserModel> userManager,
            IUserStore<UserModel> userStore,
            SignInManager<UserModel> signInManager,
            ILogger<AccountController> logger,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            EmailSender emailSender,
            IStringLocalizer<AccountController> localizer
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
            Localizer = localizer;
        }

        public async Task<ActionResult> Index()
        {
            var userVerify = await _userManager.GetUserAsync(User);
            if (userVerify == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (userVerify.EmailConfirmed == false)
            {
                return RedirectToAction("Index", "ConfirmEmail");
            }
            var users = await _userManager.Users.ToListAsync();
            var AccountViewModel = new List<AccountViewModel>();


            foreach (var user in users)
            {
                var thisViewModel = new AccountViewModel();
                var roles = await _userManager.GetRolesAsync(user);
                thisViewModel.Id = user.Id;
                thisViewModel.Name = user.Name;
                thisViewModel.EmployeeID = user.EmployeeID;
                thisViewModel.Email = user.Email;
                thisViewModel.EmailConfirmed = user.EmailConfirmed;
                thisViewModel.PhoneNumber = user.PhoneNumber;
                thisViewModel.Department = await _context.Department.FirstOrDefaultAsync(d => d.DeptNo == user.DeptNo);
                thisViewModel.ListRoles = roles;

                AccountViewModel.Add(thisViewModel);
            }

            return View(AccountViewModel);
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                string userNotFound = @Localizer["userNotFound"];
                TempData["Error"] = userNotFound;  
                return RedirectToAction(nameof(Index));
            }
            var user = await _userManager.Users.Include(u => u.Department).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                string userNotFound = @Localizer["userNotFound"];
                TempData["Error"] = userNotFound;
                return RedirectToAction(nameof(Index));
            }
            var roles = await _userManager.GetRolesAsync(user);

            var thisViewModel = new AccountViewModel();
            thisViewModel.Id = user.Id;
            thisViewModel.Name = user.Name;
            thisViewModel.EmployeeID = user.EmployeeID;
            thisViewModel.Email = user.Email;
            thisViewModel.EmailConfirmed = user.EmailConfirmed;
            thisViewModel.PhoneNumber = user.PhoneNumber;
            thisViewModel.Department = await _context.Department.FirstOrDefaultAsync(d => d.DeptNo == user.DeptNo);
            thisViewModel.ListRoles = await _userManager.GetRolesAsync(user);
            thisViewModel.Signature = user.Signature;

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
        public async Task<IActionResult> Create(AccountViewModel accountViewModel)
        {
            var department = accountViewModel.Department;
            if (ModelState.IsValid)
            {
                var user = new AccountViewModel();


                if (accountViewModel.Name != null)
                {
                    var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Name == accountViewModel.Name);
                    if (existingUser == null)
                    {
                        user.Name = accountViewModel.Name;
                    }
                    else
                    {
                        string nameExist = @Localizer["nameExist"];
                        ModelState.AddModelError(string.Empty, nameExist);
                        await PopulateDepartmentsAndRoles();
                        return View(accountViewModel);
                    }
                }
                else
                {
                    string nameReq = @Localizer["nameReq"];
                    ModelState.AddModelError(string.Empty, nameReq);
                    await PopulateDepartmentsAndRoles();
                    return View(accountViewModel);
                }
                if (accountViewModel.EmployeeID != null)
                {
                    var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.EmployeeID == accountViewModel.EmployeeID);
                    if (existingUser != null)
                    {
                        string empIDExist = @Localizer["empIDExist"];
                        ModelState.AddModelError(string.Empty, empIDExist);
                        await PopulateDepartmentsAndRoles();
                        return View(accountViewModel);
                    }
                    user.EmployeeID = accountViewModel.EmployeeID;
                }
                else
                {
                    string empIDReq = @Localizer["empIDReq"];
                    ModelState.AddModelError(string.Empty, empIDReq);
                    await PopulateDepartmentsAndRoles();
                    return View(accountViewModel);
                }
                if (accountViewModel.Email != null)
                {
                    var existingEmail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == accountViewModel.Email);
                    if (existingEmail != null)
                    {
                        string emailExist = @Localizer["emailExist"];
                        ModelState.AddModelError(string.Empty, emailExist);
                        await PopulateDepartmentsAndRoles();
                        return View(accountViewModel);
                    }
                }
                else
                {
                    string emailReq = @Localizer["emailReq"];
                    ModelState.AddModelError(string.Empty, emailReq);
                    await PopulateDepartmentsAndRoles();
                    return View(accountViewModel);
                }
                if (accountViewModel.PhoneNumber != null)
                {
                    user.PhoneNumber = accountViewModel.PhoneNumber;
                }
                else
                {
                    string phoneReq = @Localizer["phoneReq"];
                    ModelState.AddModelError(string.Empty, phoneReq);
                    await PopulateDepartmentsAndRoles();
                    return View(accountViewModel);
                }
                if (accountViewModel.DeptNo != null)
                {
                    user.Department = await _context.Department.FirstOrDefaultAsync(d => d.DeptNo == accountViewModel.DeptNo);
                }
                else
                {
                    string deptReq = @Localizer["deptReq"];
                    ModelState.AddModelError(string.Empty, deptReq);
                    await PopulateDepartmentsAndRoles();
                    return View(accountViewModel);
                }
                if (accountViewModel.ListRoles == null)
                {
                    string selectRole = @Localizer["selectRole"];
                    ModelState.AddModelError(string.Empty, selectRole);
                    await PopulateDepartmentsAndRoles();
                    return View(accountViewModel);
                }
                await _userStore.SetUserNameAsync(user, accountViewModel.EmployeeID, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, accountViewModel.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    if (accountViewModel.ListRoles.Count > 0)
                    {
                        if (accountViewModel.ListRoles == null)
                        {
                            string selectRole = @Localizer["selectRole"];
                            ModelState.AddModelError(string.Empty, selectRole);
                            await PopulateDepartmentsAndRoles();
                            return View(accountViewModel);
                        }

                        foreach (var role in accountViewModel.ListRoles)
                        {
                            var roleExists = await _roleManager.FindByIdAsync(role);
                            if (roleExists != null)
                            {
                                await _userManager.AddToRoleAsync(user, roleExists.Name);
                            }
                        }
                        if (await _userManager.GetRolesAsync(user) == null)
                        {
                            string selectRole = @Localizer["selectRole"];
                            ModelState.AddModelError(string.Empty, selectRole);
                            await PopulateDepartmentsAndRoles();
                            return View(accountViewModel);
                        }
                    }
                    else
                    {
                        string selectRole = @Localizer["selectRole"];
                        ModelState.AddModelError(string.Empty, selectRole);
                        await PopulateDepartmentsAndRoles();
                        return View(accountViewModel);
                    }

                    // Send email confirmation
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var encodedToken = Encoding.UTF8.GetBytes(token);
                    var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                    var callbackUrl = Url.Action("Index", "SetPassword", new { userId = user.Id, token = validToken }, Request.Scheme);
                    string accConfirm = @Localizer["accConfirm"];
                    var dynamicTemplateData = new { Subject = accConfirm, ConfirmLink = callbackUrl };
                    var templateId = "d-ca5e1fdee08047d1afa4448fe1cee09a";
                    await _emailSender.SendEmailAsync(user.Email, templateId, dynamicTemplateData);

                    string userCreated = @Localizer["userCreated"];
                    TempData["Success"] = userCreated;
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            await PopulateDepartmentsAndRoles();
            string userCreateF = @Localizer["userCreateF"];
            TempData["Error"] = userCreateF;
            return View(accountViewModel);
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                string userNotFound = @Localizer["userNotFound"];
                TempData["Error"] = userNotFound;
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                string userNotFound = @Localizer["userNotFound"];
                TempData["Error"] = userNotFound;
                return RedirectToAction(nameof(Index));
            }

            var roles = await _userManager.GetRolesAsync(user);
            var editUser = new EditUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                EmployeeID = user.EmployeeID,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                DeptNo = user.DeptNo,
                PhoneNumber = user.PhoneNumber,
                ListRoles = roles
            };
            var department = await _context.Department.ToListAsync();
            ViewData["Departments"] = new SelectList(department, "DeptNo", "DeptName", user.DeptNo);
            ViewData["Roles"] = _roleManager.Roles.ToList();
            return View(editUser);
        }

        //POST: Department/Edit/5 review the code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel editUser)
        {
            if (id != editUser.Id)
            {
                string userNotFound = @Localizer["userNotFound"];
                TempData["Error"] = userNotFound;
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        string userNotFound = @Localizer["userNotFound"];
                        TempData["Error"] = userNotFound;
                        return RedirectToAction(nameof(Index));
                    }
                    if (editUser.Name != user.Name)
                    {
                        if (editUser.Name != null)
                        {
                            user.Name = editUser.Name;
                        }
                    }
                    if (editUser.PhoneNumber != user.PhoneNumber)
                    {
                        if (editUser.PhoneNumber != null)
                        {
                            user.PhoneNumber = editUser.PhoneNumber;
                        }
                    }
                    if (editUser.DeptNo != null)
                    {
                        editUser.Department = await _context.Department.FirstOrDefaultAsync(d => d.DeptNo == editUser.DeptNo);
                    }
                    if (user.DeptNo != null)
                    {
                        user.Department = await _context.Department.FirstOrDefaultAsync(d => d.DeptNo == user.DeptNo);
                    }
                    if (editUser.Department != user.Department)
                    {
                        if (editUser.Department != null)
                        {
                            user.Department = editUser.Department;
                        }
                    }
                    if (editUser.EmailConfirmed != user.EmailConfirmed)
                    {
                        user.EmailConfirmed = editUser.EmailConfirmed;
                    }
                    if (editUser.PhoneNumberConfirmed != user.PhoneNumberConfirmed)
                    {
                        user.PhoneNumberConfirmed = editUser.PhoneNumberConfirmed;
                    }
                    var roles = await _userManager.GetRolesAsync(user);

                    if (editUser.ListRoles != roles)
                    {
                        if (editUser.ListRoles != null)
                        {
                            if (_userManager.GetUsersInRoleAsync("Admin").Result.Count() > 1)
                            {
                                await _userManager.RemoveFromRolesAsync(user, roles);
                            }
                            else
                            {
                                var adminRole = "Admin";
                                await _userManager.RemoveFromRolesAsync(user, roles.Except(new List<string> { adminRole }));
                                string removeAdmin = @Localizer["removeAdmin"];
                                TempData["Error"] = removeAdmin;
                            }
                            foreach (var role in editUser.ListRoles)
                            {
                                await _userManager.AddToRoleAsync(user, role);
                            }
                        }
                    }

                    if (editUser.Password != null)
                    {
                        if (editUser.ConfirmPassword == editUser.Password)
                        {
                            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                            var result = await _userManager.ResetPasswordAsync(user, token, editUser.Password);
                            if (result.Succeeded)
                            {
                                string passChangeS = @Localizer["passChangeS"];
                                TempData["Success"] = passChangeS;
                            }
                            else
                            {
                                string passChangeF = @Localizer["passChangeF"];
                                TempData["Error"] = passChangeF;
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                            }
                        }
                    }
                    await _userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(editUser.Id))
                    {
                        string userNotFound = @Localizer["userNotFound"];
                        TempData["Error"] = userNotFound;
                        return RedirectToAction(nameof(Index));
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
            return View(editUser);
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                string userNotFound = @Localizer["userNotFound"];
                TempData["Error"] = userNotFound;
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.Users.Include(u => u.Department).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                string userNotFound = @Localizer["userNotFound"];
                TempData["Error"] = userNotFound;
                return RedirectToAction(nameof(Index));
            }
            var roles = await _userManager.GetRolesAsync(user);

            var thisViewModel = new AccountViewModel();
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

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (_userManager.IsInRoleAsync(user, "Admin").Result)
                {
                    if (_userManager.GetUsersInRoleAsync("Admin").Result.Count() > 1)
                    {
                        if (user.Id == _userManager.GetUserId(User))
                        {
                            await _signInManager.SignOutAsync();
                        }
                        await _userManager.DeleteAsync(user);
                    }
                    else
                    {
                        string deleteLastAdmin = @Localizer["deleteLastAdmin"];
                        TempData["Error"] = deleteLastAdmin;
                    }
                }
                else
                {
                    await _userManager.DeleteAsync(user);
                }
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
        private bool UserExists(string id)
        {
            return _userManager.Users.Any(u => u.Id == id);
        }
    }
}
