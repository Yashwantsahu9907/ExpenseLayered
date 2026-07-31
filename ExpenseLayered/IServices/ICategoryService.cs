using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;

namespace ExpenseLayeredApi.IServices
{
    public interface ICategoryService
    {
        Task<ResponseResult<CategoryDto>> CreateCategory(CategoryDto dto, int UserId);
        Task<ResponseResult<List<CategoryDto>>> GetAllCategory(int userId);
        Task<ResponseResult<CategoryUpdateDto>> UpdateCategory(CategoryUpdateDto dto, int userId);
        Task<ResponseResult<bool>> DeleteCategory(int id, int userId);
        Task<ResponseResult<CategoryDto>> GetCategoryById(int id, int userId);
    }
}
