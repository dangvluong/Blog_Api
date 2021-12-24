using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class CategoryRepository : BaseRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IList<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category> GetCategory(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }
        public void AddRange(Category[] categories)
        {
            _context.Categories.AddRange(categories);
        }
    }
}
