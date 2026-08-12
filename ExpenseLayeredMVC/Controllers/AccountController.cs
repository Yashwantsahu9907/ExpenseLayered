using ExpenseLayeredMVC.Models;
using ExpenseLayeredMVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;   // Read JWT Token
using System.Security.Claims;

namespace ExpenseLayeredMVC.Controllers;

public class AccountController : Controller
{
    private readonly AuthApiService _authApiService;
    public AccountController(AuthApiService authApiService)
    {
        _authApiService = authApiService;
    }
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);  // if validation fail Return same page and keep entered data
        }
        var result = await _authApiService.Register(dto); // call the api

        if (result != null && result.IsSuccess)
        {
            TempData["Success"] = result.Message; //use tempdata because if i use viewbag data will be lost because after successful registration page redirect to login page

            return RedirectToAction("Login");
        }
        ViewBag.Error = result.Message;
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _authApiService.LoginUser(dto);

        if (result == null || !result.IsSuccess)
        {
            ViewBag.Error = "Invalid Email or Password";
            return View(dto);
        }

        var handler = new JwtSecurityTokenHandler();  // Read JWT Token
        var jwtToken = handler.ReadJwtToken(result.Data.Token);

        var identity = new ClaimsIdentity(  // Create Identity using JWT Claims
            jwtToken.Claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(     // Login user and create Authentication Cookie
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        Response.Cookies.Append("JwtToken", result.Data.Token); // Store JWT Token in Cookie // This token will be used while calling API
        // Get role directly from JWT Token
        var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        // Redirect according to role
        if (role == "SuperAdmin")
        {
            return RedirectToAction("Dashboard", "SuperAdmin");
        }

        if (role == "Admin")
        {
            return RedirectToAction("Dashboard", "Admin");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);  // Remove Authentication Cookie
        Response.Cookies.Delete("JwtToken");  // Remove JWT Cookie
        return RedirectToAction("Login", "Account");
    }
}