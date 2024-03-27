using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Triplite_Committee_Platform.Models;

namespace Triplite_Committee_Platform.Controllers
{
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

        public async Task<IActionResult> ChooseRole(ChooseRoleViewModel model) // commented validation for testing
        {
            //if (User?.Identity?.IsAuthenticated == false)
            //{
            //    return RedirectToPage("/Account/Login"); // Redirect to login if not authenticated
            //}

            var user = await _userManager.GetUserAsync(User);
            //if (user == null)
            //{
            //    return NotFound($"Unable to load user with ID '{User.FindFirstValue(ClaimTypes.NameIdentifier)}'.");
            //}

            var roles = await _userManager.GetRolesAsync(user);

            var viewModel = new ChooseRoleViewModel(_httpContextAccessor);

            viewModel.Roles = roles;

            viewModel.SelectedRole = model.SelectedRole;
            viewModel.SetRoleCookie(model.SelectedRole);

            return View(viewModel);
        }
    }
}