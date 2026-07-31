using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseLayeredApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [Authorize]
    [HttpPost("CreateCategory")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);  // generate token match and stor
        if(userIdClaim == null)
        {
            return Unauthorized("UserId Claim not found");
        }
        int userId = int.Parse(userIdClaim.Value);
        var result = await _categoryService.CreateCategory(dto, userId);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("GetCategory")]
    public async Task<IActionResult> GetAllCategory()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("UserId claim not found.");
        }
        int userId = int.Parse(userIdClaim.Value);
        var result = await _categoryService.GetAllCategory(userId);
        return Ok(result);
    }
}
