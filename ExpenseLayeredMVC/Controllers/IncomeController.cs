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

        if (result != null && result.Data != null)
        {
            return View(result.Data);
        }
        return View(new List<IncomeDto>());
    }

    // Open Create Income Page
    public IActionResult Create()
    {
        var dto = new IncomeDto
        {
            IncomeDate = DateTime.Today  // present date by default
        };
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(IncomeDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }
        var result = await _incomeService.CreateIncomeAsync(dto);
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
            ViewBag.Error = "Income not created.";
        }
        return View(dto);
    }

    // Open Edit Page
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _incomeService.GetIncomeByIdAsync(id);
        if (result == null || !result.IsSuccess || result.Data == null)
        {
            return NotFound();
        }
        // Copy data into Update DTO
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
        if (!ModelState.IsValid)
        {
            return View(dto);
        }
        var result = await _incomeService.UpdateIncomeAsync(dto);
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
            ViewBag.Error = "Income not updated.";
        }
        return View(dto);
    }

    // Delete Income
    public async Task<IActionResult> Delete(int id)
    {
        await _incomeService.DeleteIncomeAsync(id);
        return RedirectToAction("Index");
    }
}