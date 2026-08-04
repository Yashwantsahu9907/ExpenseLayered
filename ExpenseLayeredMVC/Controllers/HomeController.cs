using ExpenseLayeredMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseLayeredMVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ExpenseApiService _expenseService;
    private readonly IncomeApiService _incomeService;

    public HomeController(ExpenseApiService expenseService, IncomeApiService incomeService)
    {
        _expenseService = expenseService;
        _incomeService = incomeService;
    }

    public async Task<IActionResult> Index()
    {
        var expenseResult = await _expenseService.GetAllExpensesAsync();
        var incomeResult = await _incomeService.GetAllIncomesAsync();

        var expenses = expenseResult?.Data ?? new();
        var incomes = incomeResult?.Data ?? new();

        var totalExpense = expenses.Sum(e => e.Amount);
        var totalIncome = incomes.Sum(i => i.Amount);
        var remainingBalance = totalIncome - totalExpense;

        ViewBag.TotalExpense = totalExpense;
        ViewBag.TotalIncome = totalIncome;
        ViewBag.RemainingBalance = remainingBalance;
        
        ViewBag.RecentExpenses = expenses.OrderByDescending(e => e.Id).Take(5).ToList();
        ViewBag.RecentIncomes = incomes.OrderByDescending(i => i.IncomeDate).Take(5).ToList();

        return View();
    }
}
