using ExpenseLayeredMVC.GenericResponse;
using ExpenseLayeredMVC.Models;

namespace ExpenseLayeredMVC.Services;

public class ExpenseApiService : BaseApiService
{
    // Constructor
    public ExpenseApiService(HttpClient httpClient, IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor) : base(httpClient, configuration, httpContextAccessor)
    {
    }

    // Get All Expenses
    public async Task<ResponseResult<List<ExpenseDto>>> GetAllExpensesAsync()
    {
        AddAuthorizationHeader();
        var result = await _httpClient.GetFromJsonAsync<ResponseResult<List<ExpenseDto>>>(_baseUrl + "Expense/GetAllExpense");
        return result;
    }

    // Get Expense By Id
    public async Task<ResponseResult<ExpenseDto>> GetExpenseByIdAsync(int id)
    {
        AddAuthorizationHeader();
        var result = await _httpClient.GetFromJsonAsync<ResponseResult<ExpenseDto>>(
            _baseUrl + "Expense/GetExpenseById/" + id);
        return result;
    }

    // Create Expense
    public async Task<ResponseResult<ExpenseDto>> CreateExpenseAsync(ExpenseDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PostAsJsonAsync(
            _baseUrl + "Expense/CreateExpense", dto);
        // Convert JSON into C# Object
        var result = await response.Content.ReadFromJsonAsync<ResponseResult<ExpenseDto>>();
        return result;
    }

    // Update Expense
    public async Task<ResponseResult<ExpenseUpdateDto>> UpdateExpenseAsync(ExpenseUpdateDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PutAsJsonAsync(
            _baseUrl + "Expense/UpdateExpense", dto);
        var result = await response.Content.ReadFromJsonAsync<ResponseResult<ExpenseUpdateDto>>();
        return result;
    }

    // Delete Expense
    public async Task<ResponseResult<bool>> DeleteExpenseAsync(int id)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync(_baseUrl + "Expense/DeleteExpense/" + id);
        var result = await response.Content.ReadFromJsonAsync<ResponseResult<bool>>();
        return result;
    }
}