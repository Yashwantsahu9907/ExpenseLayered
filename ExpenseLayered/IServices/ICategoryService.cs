using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;

namespace ExpenseLayeredApi.IServices
{
    public interface ICategoryService
    {
        Task<ResponseResult<CategoryDto>> CreateCategory(CategoryDto dto, int userId);
        Task<ResponseResult<List<CategoryDto>>> GetAllCategory(int? userId);
        Task<ResponseResult<CategoryUpdateDto>> UpdateCategory(CategoryUpdateDto dto, int? targetUserId, int updatedBy);
        Task<ResponseResult<bool>> DeleteCategory(int id, int? targetUserId, int deletedBy);
        Task<ResponseResult<CategoryDto>> GetCategoryById(int id, int? userId);
    }
}
