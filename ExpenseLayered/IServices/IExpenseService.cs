using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;

namespace ExpenseLayeredApi.IServices
{
    public interface IExpenseService
    {
        Task<ResponseResult<ExpenseDto>> CreateExpense(ExpenseDto dto, int userId);
        Task<ResponseResult<ExpenseUpdateDto>> UpdateExpense(ExpenseUpdateDto dto, int userId);
        Task<ResponseResult<bool>> DeleteExpense(int id, int userId);
    }
}
