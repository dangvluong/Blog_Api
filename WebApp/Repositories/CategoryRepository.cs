using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(HttpClient client) : base(client)
        {

        }
        public async Task<List<Category>> GetCategories()
        {                   
            var response = await Send<List<Category>>("/api/category", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<List<Category>>());
            return (List<Category>)response.Data;
        }
        public async Task<Category> GetCategoryById(int id)
        {
            var response = await Send<Category>($"/api/category/{id}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<Category>());
            return (Category)response.Data;                   
        }
        public async Task<ResponseModel> Create(Category obj, string token)
        {                 
            return await Send<Category>("/api/category", obj, (client, url, obj) => client.PostAsJsonAsync<Category>(url, obj), token);
        }
        public async Task<ResponseModel> Edit(Category obj, string token)
        {                    
            return await Send<Category>($"/api/category/{obj.Id}", obj, (client, url, obj) => client.PutAsJsonAsync<Category>(url, obj), token);
        }

        public async Task<ResponseModel> Delete(int id, string token)
        {            
            return await Send($"/api/category/{id}", (client, url) => client.DeleteAsync(url), token);
        }

        public async Task<List<Category>> ManagerGetCategories(string token)
        {
            var response = await Send<List<Category>>("/api/category/managergetcategories", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<List<Category>>(),token);
            return (List<Category>)response.Data;
        }
    }
}
