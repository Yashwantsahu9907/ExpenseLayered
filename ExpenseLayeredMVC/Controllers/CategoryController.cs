using ExpenseLayeredMVC.Models;
using ExpenseLayeredMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseLayeredMVC.Controllers;

[Authorize]
public class CategoryController : Controller
{
    private readonly CategoryApiService _categoryService;
    public CategoryController(CategoryApiService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _categoryService.GetAllCategoriesAsync();  // call api
        if (result != null && result.Data != null)
        {
            return View(result.Data);
        }
        return View(new List<CategoryDto>());
    }

    public IActionResult Create() // Open Create Category Page
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }
        var result = await _categoryService.CreateCategoryAsync(dto);
        if (result != null && result.IsSuccess)
        {
            return RedirectToAction("Index");
        }
        // Show error message
        if (result != null)
        {
            ViewBag.Error = result.Message;
        }
        else
        {
            ViewBag.Error = "Category not created.";
        }
        return View(dto);
    }

    // Open Edit Page
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id);
        if (result == null || !result.IsSuccess || result.Data == null)
        {
            return NotFound();
        }
        var dto = new CategoryUpdateDto // Copy data into Update DTO
        {
            Id = result.Data.Id,
            Name = result.Data.Name
        };
        return View(dto);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(CategoryUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }
        var result = await _categoryService.UpdateCategoryAsync(dto);
        if (result != null && result.IsSuccess)
        {
            return RedirectToAction("Index");
        }
        if (result != null)
        {
            ViewBag.Error = result.Message;
        }
        else
        {
            ViewBag.Error = "Category not updated.";
        }
        return View(dto);
    }
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return RedirectToAction("Index");
    }
}