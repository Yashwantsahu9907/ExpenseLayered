using ExpenseLayeredMVC.GenericResponse;
using ExpenseLayeredMVC.Models;

namespace ExpenseLayeredMVC.Services;

public class AuthApiService
{
    private readonly HttpClient _httpClient; // used to call api
    private readonly IConfiguration _configuration; // reade baseurl from appsettingjson

    public AuthApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    // Register User
    public async Task<ResponseResult<string>> Register(RegisterDto dto)
    {
        var baseUrl = _configuration["ApiSettings:BaseUrl"]; // read baseurl
        var response = await _httpClient.PostAsJsonAsync(baseUrl + "Auth/Register", dto);  // call register api
        var result = await response.Content.ReadFromJsonAsync<ResponseResult<string>>(); // Convert JSON response into C# object
        return result;
    }

    // Login User
    public async Task<ResponseResult<LoginResponseDto>> LoginUser(LoginDto dto)
    {
        var baseUrl = _configuration["ApiSettings:BaseUrl"];
        var response = await _httpClient.PostAsJsonAsync(baseUrl + "Auth/Login", dto);
        var result = await response.Content.ReadFromJsonAsync<ResponseResult<LoginResponseDto>>();
        return result;
    }
}