using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Triplite_Committee_Platform.Models;

public class ChooseRoleViewModel
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChooseRoleViewModel( IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IList<string> Roles { get; set; }
    public string SelectedRole { get; set; }

    public IActionResult SetRoleCookie(string role) // This method is not secure should switch to Session ***IMPORTANT***
    {
        // Set the cookie
        _httpContextAccessor.HttpContext.Response.Cookies.Append("SelectedRole", role, new CookieOptions { HttpOnly = true });

        return new RedirectToActionResult("Index", "Home", null);
    }
}
