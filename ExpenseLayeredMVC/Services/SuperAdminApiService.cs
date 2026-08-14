using ExpenseLayeredMVC.GenericResponse;
using ExpenseLayeredMVC.Models;

namespace ExpenseLayeredMVC.Services;

public class SuperAdminApiService : BaseApiService
{
    // Constructor
    public SuperAdminApiService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(httpClient, configuration, httpContextAccessor)
    {
    }

    // Get All Users
    public async Task<ResponseResult<List<UserDto>>> GetAllUsersAsync()
    {
        AddAuthorizationHeader();
        var result = await _httpClient.GetFromJsonAsync<ResponseResult<List<UserDto>>>(
            _baseUrl + "User");

        return result;
    }

    // Get User By Id
    public async Task<ResponseResult<UserDto>> GetUserByIdAsync(int id)
    {
        AddAuthorizationHeader();
        var result = await _httpClient.GetFromJsonAsync<ResponseResult<UserDto>>(
            _baseUrl + "User/" + id);
        return result;
    }

    // Create User
    public async Task<ResponseResult<UserDto>> CreateUserAsync(CreateUserDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PostAsJsonAsync(
            _baseUrl + "User", dto);
        var result = await response.Content
            .ReadFromJsonAsync<ResponseResult<UserDto>>();
        return result;
    }

    // Update User
    public async Task<ResponseResult<UserDto>> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PutAsJsonAsync(
            _baseUrl + "User/" + id, dto);

        var result = await response.Content
            .ReadFromJsonAsync<ResponseResult<UserDto>>();

        return result;
    }

    // Delete User
    public async Task<ResponseResult<bool>> DeleteUserAsync(int id)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync(
            _baseUrl + "User/" + id);
        var result = await response.Content
            .ReadFromJsonAsync<ResponseResult<bool>>();

        return result;
    }
}