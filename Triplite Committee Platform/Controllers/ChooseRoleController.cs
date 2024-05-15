using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using Triplite_Committee_Platform.Models;
using Triplite_Committee_Platform.ViewModels;

namespace Triplite_Committee_Platform.Controllers
{
    [Authorize]
    public class ChooseRoleController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<ChooseRoleController> Localizer;

        public ChooseRoleController(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor,IStringLocalizer<ChooseRoleController>localizer)
        {
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _userManager = userManager;
            Localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                string unableLoadUser = Localizer["unableLoadUser"];
                TempData["Message"] = unableLoadUser;
                return RedirectToAction("Index", "Home");
            }
            if (user.EmailConfirmed == false)
            {
                return RedirectToAction("Index", "ConfirmEmail");
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (roles != null && roles.Count > 1)
            {
                return RedirectToAction("ChooseRole");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> ChooseRole()
        {
            var user = await _userManager.GetUserAsync(User);

            var roles = await _userManager.GetRolesAsync(user);

            var viewModel = new ChooseRoleViewModel
            {
                Roles = roles.ToList()
            };

            ViewData["Roles"] = new SelectList(viewModel.Roles);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChooseRole(ChooseRoleViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.SelectedRole))
            {
                string selectRole = Localizer["selectRole"];
                TempData["Message"]= selectRole;
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                string unableLoadUser = Localizer["unableLoadUser"];
                TempData["Message"] = unableLoadUser;
                return RedirectToAction(nameof(Index));
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count < 2)
            {
                string multiRoles = Localizer["multiRoles"];
                TempData["Message"] = multiRoles;
                return RedirectToAction(nameof(Index));
            }
            if (!roles.Contains(model.SelectedRole))
            {
                string noPermission = Localizer["noPermission"];
                TempData["Message"] = noPermission;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                HttpContext.Session.SetString("ActiveRole", model.SelectedRole);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}