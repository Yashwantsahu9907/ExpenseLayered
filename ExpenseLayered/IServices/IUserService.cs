using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;

namespace ExpenseLayeredApi.IServices;

public interface IUserService
{
    Task<ResponseResult<List<UserDto>>> GetAllUsers();
    Task<ResponseResult<UserDto>> GetUserById(int id);
    Task<ResponseResult<UserDto>> CreateUser(CreateUserDto dto);
    Task<ResponseResult<UserDto>> UpdateUser(int id, UpdateUserDto dto);
    Task<ResponseResult<bool>> DeleteUser(int id);
}