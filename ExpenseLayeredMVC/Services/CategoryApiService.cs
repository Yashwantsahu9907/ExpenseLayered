using ExpenseLayeredMVC.GenericResponse;
using ExpenseLayeredMVC.Models;

namespace ExpenseLayeredMVC.Services;

public class CategoryApiService : BaseApiService
{
    // Constructor
    public CategoryApiService(
        HttpClient httpClient,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
        : base(httpClient, configuration, httpContextAccessor)
    {
    }

    // Get All Categories
    public async Task<ResponseResult<List<CategoryDto>>> GetAllCategoriesAsync()
    {
        AddAuthorizationHeader(); // Add JWT Token in Request Header
        var result = await _httpClient.GetFromJsonAsync<ResponseResult<List<CategoryDto>>>(
            _baseUrl + "Category/GetCategory");
        return result;
    }

    // Get Category By Id
    public async Task<ResponseResult<CategoryDto>> GetCategoryByIdAsync(int id)
    {
        AddAuthorizationHeader();
        var result = await _httpClient.GetFromJsonAsync<ResponseResult<CategoryDto>>(
            _baseUrl + "Category/GetCategoryById/" + id);
        return result;
    }

    // Create Category
    public async Task<ResponseResult<CategoryDto>> CreateCategoryAsync(CategoryDto dto)
    {
        AddAuthorizationHeader();

        var response = await _httpClient.PostAsJsonAsync(
            _baseUrl + "Category/CreateCategory", dto);
        // Convert JSON into C# Object
        var result = await response.Content.ReadFromJsonAsync<ResponseResult<CategoryDto>>();
        return result;
    }

    // Update Category
    public async Task<ResponseResult<CategoryUpdateDto>> UpdateCategoryAsync(CategoryUpdateDto dto)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.PutAsJsonAsync(_baseUrl + "Category/CategoryUpdate", dto);
        // Convert JSON into C# Object
        var result = await response.Content.ReadFromJsonAsync<ResponseResult<CategoryUpdateDto>>();
        return result;
    }

    // Delete Category
    public async Task<ResponseResult<bool>> DeleteCategoryAsync(int id)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync(
            _baseUrl + "Category/DeleteCategory/" + id);
        var result = await response.Content.ReadFromJsonAsync<ResponseResult<bool>>();
        return result;
    }
}