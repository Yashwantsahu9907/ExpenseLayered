using ExpenseLayeredApi.Data;
using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.Entities;
using ExpenseLayeredApi.GenericResponse;
using ExpenseLayeredApi.IServices;
using Microsoft.EntityFrameworkCore;

namespace ExpenseLayeredApi.Services;

public class IncomeService : IIncomeService
{
    private readonly AppDbContext _context;
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
    public async Task<ResponseResult<IncomeUpdateDto>> UpdateIncome(IncomeUpdateDto dto, int userId)
    {
        try
        {
            var income = await _context.Incomes.FirstOrDefaultAsync(x =>  x.UserId == userId && x.Id == dto.Id && !x.IsDeleted);
            if(income == null)
            {
                return new ResponseResult<IncomeUpdateDto>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Income Not Found"
                };
            }
            income.Id = dto.Id;
            income.Title = dto.Title;
            income.Amount = dto.Amount;
            income.IncomeDate = dto.IncomeDate;
            income.UpdatedAt = DateTime.UtcNow;
            income.UpdatedBy = userId;
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
}
