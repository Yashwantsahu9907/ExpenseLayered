using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.Entities.Identity;
using ExpenseLayeredApi.IServices;
using ExpenseLayeredApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseLayeredApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    public ExpenseController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpPost("CreateExpense")]
    public async Task<IActionResult> CreateExpense([FromBody]ExpenseDto dto , int? userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);  // match claim
        if (userIdClaim == null)
        {
            return Unauthorized("UserId claim not found.");
        }
        int loggedInUserId = int.Parse(userIdClaim.Value);
        if (User.IsInRole("User"))
        {
            userId = loggedInUserId;
        }
        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
            if (userId == null)
            {
                userId = loggedInUserId;
            }
        }
        var result = await _expenseService.CreateExpense(dto, userId.Value);
        return Ok(result);
    }

    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpPut("UpdateExpense")]
    public async Task<IActionResult> UpdateExpense([FromBody]ExpenseUpdateDto dto, int? userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("UserId claim not found.");
        }
        int loggedInUserId = int.Parse(userIdClaim.Value);
        // Normal User can update only his own expense
        if (User.IsInRole("User"))
        {
            userId = loggedInUserId;
        }
        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
        }
        var result = await _expenseService.UpdateExpense(dto, userId, loggedInUserId);
        return Ok(result);
    }

    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpDelete("DeleteExpense/{id}")]
    public async Task<IActionResult> DeleteExpense(int id, int? userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("UserId claim not found.");
        }
        int loggedInUserId = int.Parse(userIdClaim.Value);
        if (User.IsInRole("User"))
        {
            userId = loggedInUserId;
        }
        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
        }
        var result = await _expenseService.DeleteExpense(id, userId, loggedInUserId);
        return Ok(result);
    }


    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpGet("GetExpenseById/{id}")]
    public async Task<IActionResult> GetExpenseById(int id, int? userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("UserId claim not found.");
        }
        int loggedInUserId = int.Parse(userIdClaim.Value);
        if (User.IsInRole("User"))
        {
            userId = loggedInUserId;
        }

        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
        }
        var result = await _expenseService.GetExpenceById(id, userId);
        return Ok(result);
    }

    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpGet("GetAllExpense")]
    public async Task<IActionResult> GetAllExpence(int ? userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("UserId claim not found.");
        }
        int loggedInUserId = int.Parse(userIdClaim.Value);

        if (User.IsInRole("User"))
        {
            userId = loggedInUserId;
        }

        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
            userId = null;
        }
        var result = await _expenseService.GetAllExpence(userId);
        return Ok(result);
    }
}
