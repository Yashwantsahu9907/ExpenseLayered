using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.Entities.Identity;
using ExpenseLayeredApi.GenericResponse;

namespace ExpenseLayeredApi.IServices
{
    public interface IAuthService
    {
        Task<ResponseResult<LoginResponseDto>> LoginUser(LoginDto dto);
        Task<ResponseResult<User>> RegisterUser(RegisterDto dto);
    }
}
