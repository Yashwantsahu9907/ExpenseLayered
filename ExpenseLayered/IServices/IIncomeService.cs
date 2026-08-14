using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;

namespace ExpenseLayeredApi.IServices
{
    public interface IIncomeService
    {
        Task<ResponseResult<IncomeDto>> CreateIncome(IncomeDto dto, int userId);
        Task<ResponseResult<IncomeUpdateDto>> UpdateIncome(IncomeUpdateDto dto, int? targetUserId, int updatedBy);
        Task<ResponseResult<bool>> DeleteIncome(int id, int? targetUserId, int deletedBy);
        Task<ResponseResult<IncomeDto>> GetIncomeById(int id, int? userId);
        Task<ResponseResult<List<IncomeDto>>> GetAllIncome(int? userId);
    }
}
