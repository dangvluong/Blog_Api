using System.Net.Http.Headers;

namespace WebApp.Models
{
    public class CategoryRepository : BaseRepository
    {
        public CategoryRepository(HttpClient client) : base(client)
        {

        }
        public async Task<List<Category>> GetCategories()
        {
            return await Get<List<Category>>("/api/category");            
        }
        public async Task<Category> GetCategoryById(int id)
        {
            return await Get<Category>($"/api/category/{id}");            
        }
        public async Task<int> Create(Category category, string token)
        {
            return await Post<Category>("/api/category", category, token);            
        }
        public async Task<int> Edit(Category category, string token)
        {
            return await Put<Category>("/api/category", category, token);            
        }

        public async Task<int> Delete(int id, string token)
        {
            return await Delete($"/api/category/{id}", token);            
        }
    }
}
