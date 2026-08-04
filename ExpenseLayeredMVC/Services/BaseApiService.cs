using System.Net.Http.Headers;

namespace ExpenseLayeredMVC.Services;

public abstract class BaseApiService // abstract calss obj not created directly
{
    // use protected instead of private because when use private child class not able to access
    protected readonly HttpClient _httpClient;  
    protected readonly IConfiguration _configuration;  // read value from appsetting json
    protected readonly IHttpContextAccessor _httpContextAccessor;  // Used to access HttpContext and Cookies
    protected readonly string _baseUrl; // Store API Base URL

    public BaseApiService(
        HttpClient httpClient,  // httpclient is a obj Used to call Web API
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;

        // Read Base URL from appsettings.json
        _baseUrl = _configuration["ApiSettings:BaseUrl"];

        // If Base URL is missing, use default URL
        if (string.IsNullOrEmpty(_baseUrl))
        {
            _baseUrl = "https://localhost:7118/api/";
        }
    }

    // Add JWT Token into Authorization Header
    protected void AddAuthorizationHeader()
    {
        var token = _httpContextAccessor.HttpContext.Request.Cookies["JwtToken"];   // Read JWT Token from Cookie
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}