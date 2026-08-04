using ExpenseLayeredMVC.Models;
using ExpenseLayeredMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseLayeredMVC.Controllers;

[Authorize]
public class IncomeController : Controller
{
    private readonly IncomeApiService _incomeService;

    public IncomeController(IncomeApiService incomeService)
    {
        _incomeService = incomeService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _incomeService.GetAllIncomesAsync();
        return View(result?.Data ?? new List<IncomeDto>());
    }

    public IActionResult Create()
    {
        var dto = new IncomeDto { IncomeDate = DateTime.Today };
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(IncomeDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        var result = await _incomeService.CreateIncomeAsync(dto);
        if (result != null && result.IsSuccess) return RedirectToAction(nameof(Index));
        ViewBag.Error = result?.Message ?? "Error creating income.";
        return View(dto);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _incomeService.GetIncomeByIdAsync(id);
        if (result == null || !result.IsSuccess || result.Data == null) return NotFound();
        
        var dto = new IncomeUpdateDto
        {
            Id = result.Data.Id,
            Title = result.Data.Title,
            Amount = result.Data.Amount,
            IncomeDate = result.Data.IncomeDate
        };
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(IncomeUpdateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        var result = await _incomeService.UpdateIncomeAsync(dto);
        if (result != null && result.IsSuccess) return RedirectToAction(nameof(Index));
        ViewBag.Error = result?.Message ?? "Error updating income.";
        return View(dto);
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _incomeService.DeleteIncomeAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
