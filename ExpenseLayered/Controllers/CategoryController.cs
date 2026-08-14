using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.IServices;
using Microsoft.AspNetCore.Authorization;
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


    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpPost("CreateCategory")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);  // generate token ko  match and stor
        if (userIdClaim == null)
        {
            return Unauthorized("UserId Claim not found");
        }

        int loggedInUserId = int.Parse(userIdClaim.Value);
        int targetUserId;
        // Admin and SuperAdmin can create category for any user or themselves
        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
            targetUserId = dto.UserId ?? loggedInUserId;   
        }
        else
        {
            targetUserId = loggedInUserId;
        }

        var result = await _categoryService.CreateCategory(dto, targetUserId);
        return Ok(result);
    }


    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpGet("GetCategory")]
    public async Task<IActionResult> GetAllCategory()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("UserId claim not found.");
        }
        int userId = int.Parse(userIdClaim.Value);
        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
            var result = await _categoryService.GetAllCategory(null);
            return Ok(result);
        }
        var userResult = await _categoryService.GetAllCategory(userId);
        return Ok(userResult);
    }


    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpPut("CategoryUpdate")]
    public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("UserId claim not found.");
        }

        int loggedInUserId = int.Parse(userIdClaim.Value);
        int? targetUserId;
        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
            targetUserId = dto.UserId;
        }
        else
        {
            targetUserId = loggedInUserId;
        }
        var result = await _categoryService.UpdateCategory(dto, targetUserId, loggedInUserId);
        return Ok(result);
    }


    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpDelete("DeleteCategory/{id}/{targetUserId?}")]
    public async Task<IActionResult> DeleteCategory(int id, int? targetUserId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("UserId claim not found.");
        }

        int loggedInUserId = int.Parse(userIdClaim.Value);
        int? categoryUserId;
        // Admin and SuperAdmin can delete any user category
        if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
            categoryUserId = targetUserId;
        }
        else
        {
            categoryUserId = loggedInUserId;
        }

        var result = await _categoryService.DeleteCategory(id, categoryUserId, loggedInUserId);
        return Ok(result);
    }


    [Authorize(Roles = "User, Admin, SuperAdmin")]
    [HttpGet("GetCategoryById/{id}/{targetUserId?}")]
    public async Task<IActionResult> GetCategoryById(int id, int? targetUserId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("UserId claim not found.");
        }

        int loggedInUserId = int.Parse(userIdClaim.Value);
        int? categoryUserId;
            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
        {
            categoryUserId = targetUserId;
        }
        else
        {
            categoryUserId = loggedInUserId;
        }
        var result = await _categoryService.GetCategoryById(id, categoryUserId);
        return Ok(result);
    }
}
