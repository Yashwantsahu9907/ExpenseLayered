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
    public ExpenseController(
        ExpenseApiService expenseService,
        CategoryApiService categoryService)
    {
        _expenseService = expenseService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _expenseService.GetAllExpensesAsync();  // call api
        if (result != null && result.Data != null)
        {
            return View(result.Data);
        }
        return View(new List<ExpenseDto>());  // if no data found
    }

    // Open Create Expense Page
    public async Task<IActionResult> Create()
    {
        await LoadCategories();  // Load category dropdown
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
            ViewBag.Error = "Expense not created.";
        }
        await LoadCategories();  // Reload dropdown
        return View(dto);
    }

    // Open Edit Page
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _expenseService.GetExpenseByIdAsync(id);
        if (result == null || !result.IsSuccess || result.Data == null)
        {
            return NotFound();
        }
        // Copy data into Update DTO
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
            ViewBag.Error = "Expense not updated.";
        }
        await LoadCategories();
        return View(dto);
    }
    
    // Delete Expense
    public async Task<IActionResult> Delete(int id)
    {
        await _expenseService.DeleteExpenseAsync(id);
        return RedirectToAction("Index");
    }

    // Load categories for dropdown
    private async Task LoadCategories()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        if (result != null && result.Data != null)
        {
            ViewBag.Categories = new SelectList(result.Data, "Id", "Name");
        }
        else
        {
            ViewBag.Categories = new SelectList(new List<CategoryDto>(), "Id", "Name");
        }
    }
}