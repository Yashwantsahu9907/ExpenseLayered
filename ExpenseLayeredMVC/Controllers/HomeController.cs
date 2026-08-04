using ExpenseLayeredMVC.Models;
using ExpenseLayeredMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseLayeredMVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ExpenseApiService _expenseService;
    private readonly IncomeApiService _incomeService;
    public HomeController(
        ExpenseApiService expenseService,
        IncomeApiService incomeService)
    {
        _expenseService = expenseService;
        _incomeService = incomeService;
    }

    public async Task<IActionResult> Index()
    {
        var expenseResult = await _expenseService.GetAllExpensesAsync();  // get all expense
        var incomeResult = await _incomeService.GetAllIncomesAsync(); // get all income frm api

        List<ExpenseDto> expenses = new List<ExpenseDto>();  // Create empty lists
        List<IncomeDto> incomes = new List<IncomeDto>();
        // If expense data exists store it
        if (expenseResult != null && expenseResult.Data != null)
        {
            expenses = expenseResult.Data;
        }
        // If income data exists, store it
        if (incomeResult != null && incomeResult.Data != null)
        {
            incomes = incomeResult.Data;
        }
        var totalExpense = expenses.Sum(x => x.Amount);  // Calculate total expense
        var totalIncome = incomes.Sum(x => x.Amount); // Calculate total income
        var remainingBalance = totalIncome - totalExpense; // Calculate remaining balance

        // Send summary data to View
        ViewBag.TotalExpense = totalExpense;
        ViewBag.TotalIncome = totalIncome;
        ViewBag.RemainingBalance = remainingBalance;

        // Get latest 5 expenses
        ViewBag.RecentExpenses = expenses
            .OrderByDescending(x => x.Id).Take(5).ToList();
        // Get latest 5 incomes
        ViewBag.RecentIncomes = incomes
            .OrderByDescending(x => x.IncomeDate).Take(5).ToList();
        return View();
    }
}