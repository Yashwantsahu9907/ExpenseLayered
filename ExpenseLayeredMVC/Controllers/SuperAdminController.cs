using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseLayeredMVC.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class SuperAdminController : Controller
{
    public IActionResult Dashboard()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);  // Remove Authentication Cookie
        Response.Cookies.Delete("JwtToken");  // Remove JWT Cookie
        return RedirectToAction("Login", "Account");
    }
}
