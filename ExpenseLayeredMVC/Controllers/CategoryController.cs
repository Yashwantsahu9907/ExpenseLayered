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
        var result = await _categoryService.GetAllCategoriesAsync();
        return View(result?.Data ?? new List<CategoryDto>());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        var result = await _categoryService.CreateCategoryAsync(dto);
        if (result != null && result.IsSuccess) return RedirectToAction(nameof(Index));
        ViewBag.Error = result?.Message ?? "Error creating category.";
        return View(dto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _categoryService.GetCategoryByIdAsync(id);
        if (result == null || !result.IsSuccess || result.Data == null) return NotFound();
        
        var dto = new CategoryUpdateDto
        {
            Id = result.Data.Id,
            Name = result.Data.Name
        };
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryUpdateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        var result = await _categoryService.UpdateCategoryAsync(dto);
        if (result != null && result.IsSuccess) return RedirectToAction(nameof(Index));
        ViewBag.Error = result?.Message ?? "Error updating category.";
        return View(dto);
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
