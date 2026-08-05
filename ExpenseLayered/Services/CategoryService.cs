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
    public async Task<ResponseResult<CategoryUpdateDto>> UpdateCategory(CategoryUpdateDto dto, int userId)
    {
        try
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == dto.Id && x.UserId == userId && !x.IsDeleted);
            if(category == null)
            {
                return new ResponseResult<CategoryUpdateDto>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Category not found"
                };
            }
               
            if(category.Name != dto.Name)
            {
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(x =>
                x.UserId == userId && x.Name.ToLower() == dto.Name.ToLower() && !x.IsDeleted);
                if (existingCategory != null)
                {
                    return ResponseResult<CategoryUpdateDto>.Conflict("Category already exists.");
                }
            }
            
            //var existingCategory = await _context.Categories.FirstOrDefaultAsync(x => x.UserId == userId && x.Name == dto.Name && x.Id != dto.Id && !x.IsDeleted);
            //if (existingCategory != null)
            //{
            //    return new ResponseResult<CategoryUpdateDto>
            //    {
            //        StatusCode = 409,
            //        IsSuccess = false,
            //        Message = "Category already exists."
            //    };
            //}
            category.Name = dto.Name; // update category
            category.UpdatedAt = DateTime.UtcNow;
            category.UpdatedBy = userId;

            await _context.SaveChangesAsync();
            return new ResponseResult<CategoryUpdateDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Category Updated Successfully",
                Data = new CategoryUpdateDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = userId
                }
            };
        }
        catch(Exception ex)
        {
            return new ResponseResult<CategoryUpdateDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    // Delete Category
    public async Task<ResponseResult<bool>> DeleteCategory(int id, int userId)
    {
        try
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if( category == null )
            {
                return new ResponseResult<bool>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Category not found"
                };
            }
            category.IsDeleted = true;
            category.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new ResponseResult<bool>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Category deleted SuccessFully"
            };
        }
        catch(Exception ex)
        {
            return new ResponseResult<bool>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    //Get category by id
    public async Task<ResponseResult<CategoryDto>> GetCategoryById(int id, int userId)
    {
        try
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id==id && x.UserId==userId && !x.IsDeleted);
            if(category == null )
            {
                return new ResponseResult<CategoryDto>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Category Not Found"
                };
            }
            return new ResponseResult<CategoryDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Category Found Successfully",
                Data = new CategoryDto
                {
                    Id = id,
                    Name = category.Name,
                }
            };
        }
        catch( Exception ex )
        {
            return new ResponseResult<CategoryDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }
}
