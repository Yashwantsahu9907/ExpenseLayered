using ExpenseLayeredApi.Data;
using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;
using ExpenseLayeredApi.Entities;
using ExpenseLayeredApi.IServices;
using Microsoft.EntityFrameworkCore;

namespace ExpenseLayeredApi.Services;

public class ExpenseService : IExpenseService
{
    private readonly AppDbContext _context;
    public ExpenseService(AppDbContext context)
    {
        _context = context;
    }

    // Create Expense
    public async Task<ResponseResult<ExpenseDto>> CreateExpense(ExpenseDto dto, int userId)
    {
        try
        {
            var expense = new Expense
            {
                Title = dto.Title,
                Amount = dto.Amount,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return new ResponseResult<ExpenseDto>
            {
                StatusCode = 201,
                IsSuccess = true,
                Message = "Expense Added",
                Data = new ExpenseDto
                {
                    Id = expense.Id,
                    Title = expense.Title,
                    Amount = expense.Amount,
                    Description = expense.Description,
                    CategoryId = expense.CategoryId
                }
            };
        }
        catch (Exception)
        {
            return new ResponseResult<ExpenseDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = "Expense not added"
            };
        }
    }

    //Update Expense
    public async Task<ResponseResult<ExpenseUpdateDto>> UpdateExpense(ExpenseUpdateDto dto, int userId)
    {
        try
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == dto.Id && x.UserId == userId && !x.IsDeleted);
            if (expense == null)
            {
                return new ResponseResult<ExpenseUpdateDto>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Expense Not Found"
                };
            }
            // Check existance of category
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == dto.CategoryId && !x.IsDeleted);
            if (category == null)
            {
                return new ResponseResult<ExpenseUpdateDto>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Category not found."
                };
            }
            expense.Title = dto.Title;
            expense.Amount = dto.Amount;
            expense.Description = dto.Description;
            expense.CategoryId = dto.CategoryId;
            expense.UpdatedAt = DateTime.UtcNow;
            expense.UpdatedBy = userId;

            await _context.SaveChangesAsync();
            return new ResponseResult<ExpenseUpdateDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Expence Updated Successfully",
                Data = dto
            };
        }
        catch (Exception ex)
        {
            return new ResponseResult<ExpenseUpdateDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

    // Delete Expense
    public async Task<ResponseResult<bool>> DeleteExpense(int id, int userId)
    {
        try
        {
            var expense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId && !x.IsDeleted);
            if (expense == null)
            {
                return new ResponseResult<bool>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Expense not Found"
                };
            }
            expense.IsDeleted = true;
            expense.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new ResponseResult<bool>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Expense deleted successfully"
            };
        }
        catch(Exception ex)
        {
            return new ResponseResult<bool>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = ex.Message,
                Data = false
            };
        }
    }
}
