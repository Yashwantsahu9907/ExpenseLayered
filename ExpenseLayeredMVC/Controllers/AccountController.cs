using ExpenseLayeredMVC.Models;
using ExpenseLayeredMVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
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
            return View(dto);

        var result = await _authApiService.Register(dto);

        if (result != null && result.IsSuccess)
        {
            TempData["Success"] = result.Message;
            return RedirectToAction("Login");
        }

        ViewBag.Error = result?.Message;
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

        if (result == null || !result.IsSuccess || string.IsNullOrEmpty(result.Data?.Token))
        {
            ViewBag.Error = result?.Message ?? "Login failed.";
            return View(dto);
        }

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(result.Data.Token);

        var claims = new List<Claim>();
        foreach (var claim in jwtToken.Claims)
        {
            claims.Add(new Claim(claim.Type, claim.Value));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        Response.Cookies.Append("JwtToken", result.Data.Token);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Cookies.Delete("JwtToken");
        return RedirectToAction("Login", "Account");
    }
}