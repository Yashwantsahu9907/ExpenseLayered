using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;

namespace ExpenseLayeredApi.IServices
{
    public interface IExpenseService
    {
        Task<ResponseResult<ExpenseDto>> CreateExpense(ExpenseDto dto, int userId);
        Task<ResponseResult<ExpenseUpdateDto>> UpdateExpense(ExpenseUpdateDto dto, int? targetUserId, int updatedBy);
        Task<ResponseResult<bool>> DeleteExpense(int id, int? targetUserId, int deletedBy);
        Task<ResponseResult<ExpenseDto>> GetExpenceById(int id, int? userId);
        Task<ResponseResult<List<ExpenseDto>>> GetAllExpence(int? userId);
    }
}
