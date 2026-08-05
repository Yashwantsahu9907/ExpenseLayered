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

    [Authorize]
    [HttpPost("CreateIncome")]
    public async Task<IActionResult> CreateIncome([FromBody] IncomeDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if(userIdClaim == null)
        {
            return Unauthorized("User Claim Not Found");
        }
        int userId = int.Parse(userIdClaim.Value);
        var result = await _incomeService.CreateIncome(dto, userId);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("UpdateIncome")]
    public async Task<IActionResult> UpdateIncome([FromBody] IncomeUpdateDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if( userIdClaim == null)
        {
            return Unauthorized("User Clain not found");
        }
        int userId = int.Parse(userIdClaim.Value);
        var result = await _incomeService.UpdateIncome(dto, userId);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("DeleteIncome/{id}")]
    public async Task<IActionResult> DeleteIncome(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("userId claim not found");
        }
        int userId = int.Parse(userIdClaim.Value);
        var result = await _incomeService.DeleteIncome(id, userId);
        return Ok(result);
    }


    [Authorize]
    [HttpGet("GetIncomeById/{id}")]
    public async Task<IActionResult> GetIncomeById(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("userId claim not found");
        }
        int userId = int.Parse(userIdClaim.Value);
        var result = await _incomeService.GetIncomeById(id, userId);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("GetAllIncome")]
    public async Task<IActionResult> GetAllIncome(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("userId claim not found");
        }
        int userId = int.Parse(userIdClaim.Value);
        var result = await _incomeService.GetAllIncome( userId);
        return Ok(result);
    }

}
