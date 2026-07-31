using ExpenseLayeredApi.Data;
using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;
using ExpenseLayeredApi.Entities;
using ExpenseLayeredApi.IServices;

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
        catch(Exception)
        {
            return new ResponseResult<ExpenseDto>
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = "Expense not added"
            };
        }
    }
}
