using ExpenseLayeredMVC.Models;
using ExpenseLayeredMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseLayeredMVC.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class SuperAdminController : Controller
{
    private readonly SuperAdminApiService _apiService;

    public SuperAdminController(SuperAdminApiService apiService)
    {
        _apiService = apiService;
    }

    // Show All Users
    public async Task<IActionResult> Index()
    {
        var result = await _apiService.GetAllUsersAsync();

        if (result == null || !result.IsSuccess)
        {
            ViewBag.Error = result?.Message ?? "Unable to get users";
            return View(new List<UserDto>());
        }

        return View(result.Data);
    }
}