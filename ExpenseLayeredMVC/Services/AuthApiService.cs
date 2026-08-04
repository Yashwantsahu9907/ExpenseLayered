using ExpenseLayeredMVC.GenericResponse;
using ExpenseLayeredMVC.Models;

namespace ExpenseLayeredMVC.Services;

public class AuthApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AuthApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<ResponseResult<string>?> Register(RegisterDto dto)
    {
        var baseUrl = _configuration["ApiSettings:BaseUrl"];

        var response = await _httpClient.PostAsJsonAsync(
            $"{baseUrl}Auth/Register",
            dto);

        return await response.Content.ReadFromJsonAsync<ResponseResult<string>>();
    }


    public async Task<ResponseResult<LoginResponseDto>> LoginUser(LoginDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "https://localhost:7118/api/Auth/Login",
            dto);

        var result = await response.Content.ReadFromJsonAsync<ResponseResult<LoginResponseDto>>();

        return result!;
    }
}