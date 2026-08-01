using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;

namespace ExpenseLayeredApi.IServices
{
    public interface IIncomeService
    {
        Task<ResponseResult<IncomeDto>> CreateIncome(IncomeDto dto, int userId);
        Task<ResponseResult<IncomeUpdateDto>> UpdateIncome(IncomeUpdateDto dto, int userId);
        Task<ResponseResult<bool>> DeleteIncome(int id, int userId);
        Task<ResponseResult<IncomeDto>> GetIncomeById(int id, int userId);
        Task<ResponseResult<List<IncomeDto>>> GetAllIncome(int id, int userId);
    }
}
