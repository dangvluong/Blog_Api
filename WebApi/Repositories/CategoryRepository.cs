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
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await FindAll(false).ToListAsync();
        }
        public async Task<Category> GetCategory(int id)
        {
            return await FindByCondition(category => category.Id == id,trackChanges: false).SingleOrDefaultAsync();
        }
        //public void Update(Category category)
        //{
        //    base.Update(category);
        //}
        //public void Add(Category category)
        //{
        //    _context.Categories.Add(category);
        //}
        #region only for seed data
        public void AddRange(Category[] categories)
        {
            _context.Categories.AddRange(categories);
        }
        #endregion

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
            //Delete(new Category { Id = categoryId });
            category.IsDeleted = true;
        }
       
    }
}
