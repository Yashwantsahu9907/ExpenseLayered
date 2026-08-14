using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseLayeredApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncomeController : ControllerBase
{
    private readonly IIncomeService _incomeService;
    public IncomeController(IIncomeService incomeService)
    {
        _incomeService = incomeService;
    }

    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpPost("CreateIncome")]
    public async Task<IActionResult> CreateIncome([FromBody] IncomeDto dto,int? userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if(userIdClaim == null)
        {
            return Unauthorized("User Claim Not Found");
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
        var result = await _incomeService.CreateIncome(dto, userId.Value);
        return Ok(result);
    }

    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpPut("UpdateIncome")]
    public async Task<IActionResult> UpdateIncome([FromBody] IncomeUpdateDto dto,int? userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if( userIdClaim == null)
        {
            return Unauthorized("User Clain not found");
        }
        int loggedInUserId = int.Parse(userIdClaim.Value);
        if (User.IsInRole("User"))
        {
            userId = loggedInUserId;
        }
        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
            // Allowed to pass null
        }
        var result = await _incomeService.UpdateIncome(dto, userId, loggedInUserId);
        return Ok(result);
    }

    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpDelete("DeleteIncome/{id}")]
    public async Task<IActionResult> DeleteIncome(int id,int? userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("userId claim not found");
        }
        int loggedInUserId = int.Parse(userIdClaim.Value);
        if (User.IsInRole("User"))
        {
            userId = loggedInUserId;
        }
        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
            // Allowed to pass null
        }
        var result = await _incomeService.DeleteIncome(id, userId, loggedInUserId);
        return Ok(result);
    }


    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpGet("GetIncomeById/{id}")]
    public async Task<IActionResult> GetIncomeById(int id, int? userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("userId claim not found");
        }
        int loggedInUserId = int.Parse(userIdClaim.Value);
        if (User.IsInRole("User"))
        {
            userId = loggedInUserId;
        }
        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
            // allowed to pass null
        }
        var result = await _incomeService.GetIncomeById(id, userId);
        return Ok(result);
    }

    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpGet("GetAllIncome")]
    public async Task<IActionResult> GetAllIncome(int? userId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized("UserId claim not found");
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

        var result = await _incomeService.GetAllIncome(userId);
        return Ok(result);
    }

}
