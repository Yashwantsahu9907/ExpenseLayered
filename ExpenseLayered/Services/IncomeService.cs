using ExpenseLayeredApi.Data;
using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.Entities;
using ExpenseLayeredApi.GenericResponse;
using ExpenseLayeredApi.IServices;
using Microsoft.EntityFrameworkCore;

namespace ExpenseLayeredApi.Services;

public class IncomeService : IIncomeService
{
    private readonly AppDbContext _context;  //Db Obj
    public IncomeService(AppDbContext context)
    {
        _context = context;
    }

    //Create Income
    public async Task<ResponseResult<IncomeDto>> CreateIncome(IncomeDto dto, int userId)
    {
        try
        {
            var income = new Income
            {
                Title = dto.Title,
                Amount = dto.Amount,
                IncomeDate = dto.IncomeDate,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };
            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();
            return new ResponseResult<IncomeDto>
            {
                StatusCode = 201,
                IsSuccess = true,
                Message = "Income Created Successfully",
                Data = new IncomeDto
                {
                    Id = income.Id,
                    Title = income.Title,
                    Amount = income.Amount,
                    IncomeDate = income.IncomeDate,
                    CreatedAt = income.CreatedAt
                }
            };
        }
        catch (Exception)
        {
            return new ResponseResult<IncomeDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = "Income not added"
            };
        }
    }

    // Update Income
    public async Task<ResponseResult<IncomeUpdateDto>> UpdateIncome(IncomeUpdateDto dto, int? targetUserId, int updatedBy)
    {
        try
        {
            var query = _context.Incomes.Where(x => x.Id == dto.Id && !x.IsDeleted);
            if (targetUserId.HasValue)
            {
                query = query.Where(x => x.UserId == targetUserId.Value);
            }
            var income = await query.FirstOrDefaultAsync();
            if(income == null)
            {
                return new ResponseResult<IncomeUpdateDto>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Income Not Found"
                };
            }
            income.Title = dto.Title;
            income.Amount = dto.Amount;
            income.IncomeDate = dto.IncomeDate;
            income.UpdatedAt = DateTime.UtcNow;
            income.UpdatedBy = updatedBy;
            await _context.SaveChangesAsync();
            return new ResponseResult<IncomeUpdateDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Income Updated Successfully",
                Data = dto
            };
        }
        catch (Exception ex)
        {
            return new ResponseResult<IncomeUpdateDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = ex.Message,
            };
        }
    }

    // Delete Income
    public async Task<ResponseResult<bool>> DeleteIncome(int id, int? targetUserId, int deletedBy)
    {
        try
        {
            var query = _context.Incomes.Where(x => x.Id == id && !x.IsDeleted);
            if (targetUserId.HasValue)
            {
                query = query.Where(x => x.UserId == targetUserId.Value);
            }
            var income = await query.FirstOrDefaultAsync();
            if (income == null)
            {
                return new ResponseResult<bool>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "User and income not found"
                };
            }
            income.IsDeleted = true;
            income.UpdatedAt = DateTime.UtcNow;
            income.UpdatedBy = deletedBy;
            await _context.SaveChangesAsync();
            return new ResponseResult<bool>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Income Deleted successfullly"
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

    // Get income by id
    public async Task<ResponseResult<IncomeDto>> GetIncomeById(int id, int? userId)
    {
        try
        {
            var query = _context.Incomes.AsNoTracking().Where(x => x.Id == id && !x.IsDeleted);
            if (userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId.Value);   // If userId is provided, filter by that user
            }
            var income = await query.FirstOrDefaultAsync();
            if (income == null)
            {
                return new ResponseResult<IncomeDto>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Income not found"
                };
            }
            return new ResponseResult<IncomeDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "income found successfully",
                Data = new IncomeDto
                {
                    Id = income.Id,
                    Title = income.Title,
                    Amount = income.Amount,
                    IncomeDate = income.IncomeDate,
                    CreatedAt = income.CreatedAt
                }
            };
        }
        catch (Exception ex)
        {
            return new ResponseResult<IncomeDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    // Get All Income
    public async Task<ResponseResult<List<IncomeDto>>> GetAllIncome(int? userId)
    {
        try
        {
            var query = _context.Incomes.Where(x => !x.IsDeleted);
            if (userId.HasValue)
            {
                query = query.Where(x => x.UserId == userId.Value);
            }

            var income = await query.AsNoTracking().Select(x => new IncomeDto
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Title = x.Title,
                    IncomeDate = x.IncomeDate,
                    CreatedAt = x.CreatedAt
                }).ToListAsync();
            if (!income.Any())
            {
                return new ResponseResult<List<IncomeDto>>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "No income found"
                };
            }
            return new ResponseResult<List<IncomeDto>>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "All income found successfully",
                Data = income
            };
        }
        catch (Exception ex)
        {
            return new ResponseResult<List<IncomeDto>>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

}
