using ExpenseLayeredMVC.GenericResponse;
using ExpenseLayeredMVC.Models;

namespace ExpenseLayeredMVC.Services;

public class IncomeApiService : BaseApiService
{
    // Constructor
    public IncomeApiService(HttpClient httpClient, IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor) : base(httpClient, configuration, httpContextAccessor)
    {
    }

    // Get All Incomes
    public async Task<ResponseResult<List<IncomeDto>>> GetAllIncomesAsync()
    {
        AddAuthorizationHeader();
        // API expects an id parameter, so we send id = 0
        var result = await _httpClient.GetFromJsonAsync<ResponseResult<List<IncomeDto>>>(_baseUrl + "Income/GetAllIncome?id=0");
        return result;
    }

    // Get Income By Id
    public async Task<ResponseResult<IncomeDto>> GetIncomeByIdAsync(int id)
    {
        AddAuthorizationHeader();
        var result = await _httpClient.GetFromJsonAsync<ResponseResult<IncomeDto>>(
            _baseUrl + "Income/GetIncomeById/" + id);
        return result;
    }

    // Create Income
    public async Task<ResponseResult<IncomeDto>> CreateIncomeAsync(IncomeDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PostAsJsonAsync(
            _baseUrl + "Income/CreateIncome", dto);
        var result = await response.Content.ReadFromJsonAsync<ResponseResult<IncomeDto>>();
        return result;
    }

    // Update Income
    public async Task<ResponseResult<IncomeUpdateDto>> UpdateIncomeAsync(IncomeUpdateDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PutAsJsonAsync(
            _baseUrl + "Income/UpdateIncome", dto);
        var result = await response.Content.ReadFromJsonAsync<ResponseResult<IncomeUpdateDto>>();
        return result;
    }

    // Delete Income
    public async Task<ResponseResult<bool>> DeleteIncomeAsync(int id)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync(
            _baseUrl + "Income/DeleteIncome/" + id);
        var result = await response.Content.ReadFromJsonAsync<ResponseResult<bool>>();
        return result;
    }
}