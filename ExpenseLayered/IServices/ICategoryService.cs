using ExpenseLayeredApi.DTO;
using ExpenseLayeredApi.GenericResponse;

namespace ExpenseLayeredApi.IServices
{
    public interface ICategoryService
    {
        Task<ResponseResult<CategoryDto>> CreateCategory(CategoryDto dto, int UserId);
        Task<ResponseResult<List<CategoryDto>>> GetAllCategory(int userId);
    }
}
