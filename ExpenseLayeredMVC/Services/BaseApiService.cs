using System.Net.Http.Headers;

namespace ExpenseLayeredMVC.Services;

public abstract class BaseApiService
{
    protected readonly HttpClient _httpClient;
    protected readonly IConfiguration _configuration;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly string _baseUrl;

    protected BaseApiService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _baseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7118/api/";
    }

    protected void AddAuthorizationHeader()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
