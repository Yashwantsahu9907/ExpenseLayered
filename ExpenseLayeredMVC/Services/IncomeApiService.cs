using ExpenseLayeredMVC.GenericResponse;
using ExpenseLayeredMVC.Models;

namespace ExpenseLayeredMVC.Services;

public class IncomeApiService : BaseApiService
{
    public IncomeApiService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(httpClient, configuration, httpContextAccessor)
    {
    }

    public async Task<ResponseResult<List<IncomeDto>>?> GetAllIncomesAsync()
    {
        AddAuthorizationHeader();
        // The API controller expects GetAllIncome(int id) but ignores it, we pass ?id=0
        return await _httpClient.GetFromJsonAsync<ResponseResult<List<IncomeDto>>>($"{_baseUrl}Income/GetAllIncome?id=0");
    }

    public async Task<ResponseResult<IncomeDto>?> GetIncomeByIdAsync(int id)
    {
        AddAuthorizationHeader();
        return await _httpClient.GetFromJsonAsync<ResponseResult<IncomeDto>>($"{_baseUrl}Income/GetIncomeById/{id}");
    }

    public async Task<ResponseResult<IncomeDto>?> CreateIncomeAsync(IncomeDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Income/CreateIncome", dto);
        return await response.Content.ReadFromJsonAsync<ResponseResult<IncomeDto>>();
    }

    public async Task<ResponseResult<IncomeUpdateDto>?> UpdateIncomeAsync(IncomeUpdateDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}Income/UpdateIncome", dto);
        return await response.Content.ReadFromJsonAsync<ResponseResult<IncomeUpdateDto>>();
    }

    public async Task<ResponseResult<bool>?> DeleteIncomeAsync(int id)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync($"{_baseUrl}Income/DeleteIncome/{id}");
        return await response.Content.ReadFromJsonAsync<ResponseResult<bool>>();
    }
}
