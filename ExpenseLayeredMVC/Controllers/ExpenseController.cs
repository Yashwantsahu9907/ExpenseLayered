using ExpenseLayeredMVC.Models;
using ExpenseLayeredMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseLayeredMVC.Controllers;

[Authorize]
public class ExpenseController : Controller
{
    private readonly ExpenseApiService _expenseService;
    private readonly CategoryApiService _categoryService;

    public ExpenseController(ExpenseApiService expenseService, CategoryApiService categoryService)
    {
        _expenseService = expenseService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _expenseService.GetAllExpensesAsync();
        return View(result?.Data ?? new List<ExpenseDto>());
    }

    public async Task<IActionResult> Create()
    {
        await LoadCategories();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ExpenseDto dto)
    {
        if (!ModelState.IsValid) 
        {
            await LoadCategories();
            return View(dto);
        }
        var result = await _expenseService.CreateExpenseAsync(dto);
        if (result != null && result.IsSuccess) return RedirectToAction(nameof(Index));
        
        ViewBag.Error = result?.Message ?? "Error creating expense.";
        await LoadCategories();
        return View(dto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _expenseService.GetExpenseByIdAsync(id);
        if (result == null || !result.IsSuccess || result.Data == null) return NotFound();
        
        var dto = new ExpenseUpdateDto
        {
            Id = result.Data.Id,
            Title = result.Data.Title,
            Amount = result.Data.Amount,
            Description = result.Data.Description,
            CategoryId = result.Data.CategoryId
        };
        await LoadCategories();
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ExpenseUpdateDto dto)
    {
        if (!ModelState.IsValid) 
        {
            await LoadCategories();
            return View(dto);
        }
        var result = await _expenseService.UpdateExpenseAsync(dto);
        if (result != null && result.IsSuccess) return RedirectToAction(nameof(Index));
        
        ViewBag.Error = result?.Message ?? "Error updating expense.";
        await LoadCategories();
        return View(dto);
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _expenseService.DeleteExpenseAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategories()
    {
        var cats = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Categories = new SelectList(cats?.Data ?? new List<CategoryDto>(), "Id", "Name");
    }
}
