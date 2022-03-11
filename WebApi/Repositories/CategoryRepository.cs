using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Category>> ManagerGetCategories(bool trackChanges)
        {
            return await FindAll(trackChanges).ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetCategories(bool trackChanges)
        {
            return await FindByCondition(c => c.IsDeleted == false,trackChanges).ToListAsync();
        }

        public async Task<Category> GetCategory(int id, bool trackChanges)
        {
            return await FindByCondition(category => category.Id == id, trackChanges).Include(c =>c.ParentCategory).SingleOrDefaultAsync();
        }

        public void AddRange(Category[] categories)
        {
            _context.Categories.AddRange(categories);
        }

        public void AddCategory(Category category)
        {
            Add(category);
        }

        public void UpdateCategory(Category category)
        {
            Update(category);
        }

        public void DeleteCategory(Category category)
        {           
            category.IsDeleted = !category.IsDeleted;
        }

    }
}
