using ExpenseLayeredMVC.GenericResponse;
using ExpenseLayeredMVC.Models;

namespace ExpenseLayeredMVC.Services;

public class ExpenseApiService : BaseApiService
{
    public ExpenseApiService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(httpClient, configuration, httpContextAccessor)
    {
    }

    public async Task<ResponseResult<List<ExpenseDto>>?> GetAllExpensesAsync()
    {
        AddAuthorizationHeader();
        return await _httpClient.GetFromJsonAsync<ResponseResult<List<ExpenseDto>>>($"{_baseUrl}Expense/GetAllExpense");
    }

    public async Task<ResponseResult<ExpenseDto>?> GetExpenseByIdAsync(int id)
    {
        AddAuthorizationHeader();
        return await _httpClient.GetFromJsonAsync<ResponseResult<ExpenseDto>>($"{_baseUrl}Expense/GetExpenseById/{id}");
    }

    public async Task<ResponseResult<ExpenseDto>?> CreateExpenseAsync(ExpenseDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Expense/CreateExpense", dto);
        return await response.Content.ReadFromJsonAsync<ResponseResult<ExpenseDto>>();
    }

    public async Task<ResponseResult<ExpenseUpdateDto>?> UpdateExpenseAsync(ExpenseUpdateDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}Expense/UpdateExpense", dto);
        return await response.Content.ReadFromJsonAsync<ResponseResult<ExpenseUpdateDto>>();
    }

    public async Task<ResponseResult<bool>?> DeleteExpenseAsync(int id)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync($"{_baseUrl}Expense/DeleteExpense/{id}");
        return await response.Content.ReadFromJsonAsync<ResponseResult<bool>>();
    }
}
