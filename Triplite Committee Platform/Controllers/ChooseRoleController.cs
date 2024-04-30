using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public ChooseRoleController(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Unable to load user";
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
                TempData["Message"]= "Please select a role";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                TempData["Message"] = "Unable to load user";
                return RedirectToAction(nameof(Index));
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count < 2)
            {
                TempData["Message"] = "User does not have multiple roles";
                return RedirectToAction(nameof(Index));
            }
            if (!roles.Contains(model.SelectedRole))
            {
                TempData["Message"] = "You dont have permission for this role";
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