using Microsoft.AspNetCore.Mvc;

public class AccountViewModel
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountViewModel(IHttpContextAccessor httpContextAccessor)
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
