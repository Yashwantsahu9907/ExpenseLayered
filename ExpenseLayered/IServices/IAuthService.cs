using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;

namespace ExpenseLayeredApi.IServices
{
    public interface IAuthService
    {
        Task<ResponseResult<LoginResponseDto>> LoginUser(LoginDto dto);
    }
}
