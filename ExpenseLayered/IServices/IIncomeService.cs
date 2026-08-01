using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;

namespace ExpenseLayeredApi.IServices
{
    public interface IIncomeService
    {
        Task<ResponseResult<IncomeDto>> CreateIncome(IncomeDto dto, int userId);
        Task<ResponseResult<IncomeUpdateDto>> UpdateIncome(IncomeUpdateDto dto, int userId);
    }
}
