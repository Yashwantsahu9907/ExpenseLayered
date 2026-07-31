using Microsoft.EntityFrameworkCore;
using ExpenseLayeredApi.Data;
using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.Entities;
using ExpenseLayeredApi.GenericResponse;
using ExpenseLayeredApi.IServices;

namespace ExpenseLayeredApi.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    //Create category
    public async Task<ResponseResult<CategoryDto>> CreateCategory(CategoryDto dto,int UserId)
    {
        try
        {
            // Check if same category already exists for same user
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(x =>
        x.UserId == UserId && x.Name.ToLower() == dto.Name.ToLower() && !x.IsDeleted);
            if (existingCategory != null)
            {
                return ResponseResult<CategoryDto>.Conflict("Category already exists.");
            }
            var category = new Category
            {
                Name = dto.Name,
                UserId = UserId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return new ResponseResult<CategoryDto>
            {
                StatusCode = 201,
                IsSuccess = true,
                Message = "Category Added Successfully",
                Data = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                }
            };
        }
        catch(Exception)
        {
            return new ResponseResult<CategoryDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = "Something went wrong"
            };
        }
    }

    //Get all categories
    public async Task<ResponseResult<List<CategoryDto>>>  GetAllCategory(int userId)
    {
        try
        {
            var categories = await _context.Categories.Where(x => x.UserId == userId && !x.IsDeleted).AsNoTracking()
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return new ResponseResult<List<CategoryDto>>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Categories found Successfully",
                Data = categories
            };
        }
        catch (Exception ex)
        {
            return new ResponseResult<List<CategoryDto>>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    // Update category 
}
