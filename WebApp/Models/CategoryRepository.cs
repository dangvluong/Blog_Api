namespace WebApp.Models
{
    public class CategoryRepository : BaseRepository
    {
        public CategoryRepository(HttpClient client) : base(client)
        {

        }
        public async Task<List<Category>> GetCategories()
        {

            //client.BaseAddress = ApiServer;
            HttpResponseMessage message = await client.GetAsync("/api/category");
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<List<Category>>();
            }
            return null;
        }
        public async Task<Category> GetCategoryById(int id)
        {
            //client.BaseAddress = ApiServer;
            HttpResponseMessage message = await client.GetAsync($"/api/category/{id}");
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<Category>();
            }
            return null;
        }
        public async Task<int> Create(Category category)
        {
            //client.BaseAddress = ApiServer;
            HttpResponseMessage message = await client.PostAsJsonAsync<Category>("/api/category", category);
            if (message.IsSuccessStatusCode)
                return 1;
            return 0;
        }
        public async Task<int> Edit(int id,Category category)
        {
            //client.BaseAddress = ApiServer;
            HttpResponseMessage message = await client.PutAsJsonAsync<Category>($"/api/category/{id}", category);
            if (message.IsSuccessStatusCode)
                return 1;
            return 0;
        }

        public async Task<int> Delete(int id)
        {
            //client.BaseAddress = ApiServer;
            HttpResponseMessage message = await client.DeleteAsync($"/api/category/{id}");
            if (message.IsSuccessStatusCode)
                return 1;
            return 0;
        }
    }
}
