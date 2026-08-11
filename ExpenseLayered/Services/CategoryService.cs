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
    public async Task<ResponseResult<CategoryDto>> CreateCategory(CategoryDto dto, int userId)
    {
        try
        {
            // Check if same category already exists for same user
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(x =>
                x.UserId == userId && x.Name.ToLower() == dto.Name.ToLower() && !x.IsDeleted);

            if (existingCategory != null)
            {
                return ResponseResult<CategoryDto>.Conflict(
                    "Category already exists.");
            }

            var category = new Category
            {
                Name = dto.Name,
                UserId = userId,
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
                    UserId = category.UserId
                }
            };
        }
        catch (Exception)
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
    public async Task<ResponseResult<List<CategoryDto>>> GetAllCategory(int? userId)
    {
        try
        {
            var query = _context.Categories.Where(x => !x.IsDeleted).AsNoTracking();
            // UserId present hai  sirf us user ka data
            // UserId null hai sab users ka data
            if (userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId.Value);
            }

            var categories = await query.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UserId = x.UserId
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
    public async Task<ResponseResult<CategoryUpdateDto>> UpdateCategory(CategoryUpdateDto dto, int targetUserId,
        int updatedBy)
    {
        try
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x =>
                x.Id == dto.Id && x.UserId == targetUserId && !x.IsDeleted);

            if (category == null)
            {
                return new ResponseResult<CategoryUpdateDto>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Category not found"
                };
            }

            if (category.Name != dto.Name)
            {
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(x =>
                    x.UserId == targetUserId && x.Name.ToLower() == dto.Name.ToLower() &&
                    x.Id != dto.Id && !x.IsDeleted);

                if (existingCategory != null)
                {
                    return ResponseResult<CategoryUpdateDto>.Conflict(
                        "Category already exists.");
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
            category.UpdatedBy = updatedBy;

            await _context.SaveChangesAsync();

            return new ResponseResult<CategoryUpdateDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Category Updated Successfully",
                Data = new CategoryUpdateDto
                {
                    Id = category.Id,
                    Name = category.Name
                }
            };
        }
        catch (Exception ex)
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
    public async Task<ResponseResult<bool>> DeleteCategory(int id, int targetUserId, int deletedBy)
    {
        try
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id &&
                x.UserId == targetUserId && !x.IsDeleted);

            if (category == null)
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
            category.UpdatedBy = deletedBy;

            await _context.SaveChangesAsync();

            return new ResponseResult<bool>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Category deleted Successfully"
            };
        }
        catch (Exception ex)
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
    public async Task<ResponseResult<CategoryDto>> GetCategoryById(int id, int? userId)
    {
        try
        {
            var query = _context.Categories.AsNoTracking()
                .Where(x => x.Id == id && !x.IsDeleted);

            // UserId present hai  sirf us user ki category
            // UserId null hai  kisi bhi user ki category
            if (userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId.Value);
            }

            var category = await query.FirstOrDefaultAsync();

            if (category == null)
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
                    Id = category.Id,
                    Name = category.Name,
                    UserId = category.UserId
                }
            };
        }
        catch (Exception ex)
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
