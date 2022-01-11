using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategory(int id);
        Task<IEnumerable<Category>> GetCategories();       
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        void AddRange(Category[] categories);
    }
}
