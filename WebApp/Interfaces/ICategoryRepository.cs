using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategoryById(int id);
        Task<ResponseModel> Create(Category category, string token);
        Task<ResponseModel> Edit(Category category, string token);
        Task<ResponseModel> Delete(int id, string token);
    }
}
