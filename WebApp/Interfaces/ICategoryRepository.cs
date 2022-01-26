using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategoryById(int id);
        Task<int> Create(Category category, string token);
        Task<int> Edit(Category category, string token);
        Task<int> Delete(int id, string token);
    }
}
