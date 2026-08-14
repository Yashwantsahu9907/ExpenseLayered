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


    // Get All Users
    public async Task<IActionResult> Index()
    {
        var result = await _apiService.GetAllUsersAsync();
        if (result == null) // Check if API response is null
        {
            ViewBag.Error = "Unable to get users.";
            return View(new List<UserDto>());
        }
        // Check if API request was not successful
        if (!result.IsSuccess)
        {
            ViewBag.Error = result.Message;
            return View(new List<UserDto>());
        }
        return View(result.Data);
    }


    // Get User By Id
    public async Task<IActionResult> Details(int id)
    {
        var result = await _apiService.GetUserByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        if (!result.IsSuccess)
        {
            return NotFound();
        }
        if (result.Data == null)
        {
            return NotFound();
        }

        return View(result.Data);
    }


    // Open Create User Page
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    // Create User
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        // Check Model Validation
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        // Call API
        var result = await _apiService.CreateUserAsync(dto);
        if (result == null)
        {
            ViewBag.Error = "Unable to create user.";
            return View(dto);
        }
        if (!result.IsSuccess)
        {
            ViewBag.Error = result.Message;
            return View(dto);
        }
        return RedirectToAction("Index");
    }


    // Open Edit User Page
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        // Get existing user
        var result = await _apiService.GetUserByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        if (!result.IsSuccess)
        {
            return NotFound();
        }
        if (result.Data == null)
        {
            return NotFound();
        }

        // Convert UserDto into UpdateUserDto
        var dto = new UpdateUserDto();

        dto.FirstName = result.Data.FirstName;
        dto.LastName = result.Data.LastName;
        dto.Email = result.Data.Email;
        dto.Gender = result.Data.Gender;
        dto.Role = result.Data.Role;

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
        if (result == null)
        {
            ViewBag.Error = "Unable to update user.";
            return View(dto);
        }
        if (!result.IsSuccess)
        {
            ViewBag.Error = result.Message;
            return View(dto);
        }
        return RedirectToAction("Index");
    }


    // Delete User
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _apiService.DeleteUserAsync(id);
        if (result == null)
        {
            TempData["Error"] = "Unable to delete user.";
            return RedirectToAction("Index");
        }
        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
}