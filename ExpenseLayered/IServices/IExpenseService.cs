using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;

namespace ExpenseLayeredApi.IServices
{
    public interface IExpenseService
    {
        Task<ResponseResult<ExpenseDto>> CreateExpense(ExpenseDto dto, int userId);
    }
}
