using ExpenseLayeredMVC.GenericResponse;
using ExpenseLayeredMVC.Models;

namespace ExpenseLayeredMVC.Services;

public class CategoryApiService : BaseApiService
{
    public CategoryApiService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        : base(httpClient, configuration, httpContextAccessor)
    {
    }

    public async Task<ResponseResult<List<CategoryDto>>?> GetAllCategoriesAsync()
    {
        AddAuthorizationHeader();
        return await _httpClient.GetFromJsonAsync<ResponseResult<List<CategoryDto>>>($"{_baseUrl}Category/GetCategory");
    }

    public async Task<ResponseResult<CategoryDto>?> GetCategoryByIdAsync(int id)
    {
        AddAuthorizationHeader();
        return await _httpClient.GetFromJsonAsync<ResponseResult<CategoryDto>>($"{_baseUrl}Category/GetCategoryById/{id}");
    }

    public async Task<ResponseResult<CategoryDto>?> CreateCategoryAsync(CategoryDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}Category/CreateCategory", dto);
        return await response.Content.ReadFromJsonAsync<ResponseResult<CategoryDto>>();
    }

    public async Task<ResponseResult<CategoryUpdateDto>?> UpdateCategoryAsync(CategoryUpdateDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}Category/CategoryUpdate", dto);
        return await response.Content.ReadFromJsonAsync<ResponseResult<CategoryUpdateDto>>();
    }

    public async Task<ResponseResult<bool>?> DeleteCategoryAsync(int id)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync($"{_baseUrl}Category/DeleteCategory/{id}");
        return await response.Content.ReadFromJsonAsync<ResponseResult<bool>>();
    }
}
