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

    // Show User Details
    public async Task<IActionResult> Details(int id)
    {
        var result = await _apiService.GetUserByIdAsync(id);
        if (result == null || !result.IsSuccess || result.Data == null)
        {
            return NotFound();
        }
        return View(result.Data);
    }

    // Open Create Page
    public IActionResult Create()
    {
        return View();
    }

    // Create User
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _apiService.CreateUserAsync(dto);
        if (result != null && result.IsSuccess)
        {
            return RedirectToAction("Index");
        }

        ViewBag.Error = result?.Message ?? "Unable to create user";
        return View(dto);
    }

    // Open Edit Page
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _apiService.GetUserByIdAsync(id);
        if (result == null || !result.IsSuccess || result.Data == null)
        {
            return NotFound();
        }

        var dto = new UpdateUserDto
        {
            FirstName = result.Data.FirstName,
            LastName = result.Data.LastName,
            Email = result.Data.Email,
            Gender = result.Data.Gender,
            Role = result.Data.Role
        };
        return View(dto);
    }

    // Update User
    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result = await _apiService.UpdateUserAsync(id, dto);
        if (result != null && result.IsSuccess)
        {
            return RedirectToAction("Index");
        }

        ViewBag.Error = result?.Message ?? "Unable to update user";
        return View(dto);
    }

    // Delete User
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _apiService.DeleteUserAsync(id);
        if (result != null && !result.IsSuccess)
        {
            TempData["Error"] = result.Message;
        }
        return RedirectToAction("Index");
    }
}