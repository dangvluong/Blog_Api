using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategory(int id, bool trackChanges);
        Task<IEnumerable<Category>> GetCategories(bool trackChanges);       
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        void AddRange(Category[] categories);
    }
}
