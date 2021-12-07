
using System.Net.Http.Headers;

namespace WebApp.Models
{
    public class PostRepository : BaseRepository
    {
        public PostRepository(HttpClient client) : base(client)
        {
        }

        public async Task<List<Post>> GetPosts()
        {
            try
            {
                //client.BaseAddress = ApiServer;
                HttpResponseMessage message = await client.GetAsync("/api/post");
                if (message.IsSuccessStatusCode)
                {
                    return await message.Content.ReadAsAsync<List<Post>>();
                }
                return new List<Post>();
            }
            catch
            {
                //Write to log
                return new List<Post>();
            }
        }

        public async Task<Post> GetPostById(int id)
        {
            
               // client.BaseAddress = ApiServer;
                HttpResponseMessage message = await client.GetAsync($"/api/post/{id}");
                if (message.IsSuccessStatusCode)
                {
                    return await message.Content.ReadAsAsync<Post>();
                }
                return null;            
          
        }

        public async Task<int> Create(Post post, string token)
        {
            //client.BaseAddress = ApiServer;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            post.DateCreated = DateTime.Now;
            post.AuthorId = 1;            
            HttpResponseMessage message = await client.PostAsJsonAsync<Post>("/api/post", post);           
            if (message.IsSuccessStatusCode)
            {
                return 1;
            }
            return 0;

        }
        public async Task<int> Edit(Post post, string token)
        {
            //client.BaseAddress = ApiServer;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            post.DateModifier = DateTime.Now;
            HttpResponseMessage message = await client.PutAsJsonAsync<Post>("/api/post", post);            
            if (message.IsSuccessStatusCode)
            {
                return 1;
            }
            return 0;
        }
        public async Task<int> Delete(int id, string token)
        {
            //client.BaseAddress = ApiServer;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage message = await client.DeleteAsync($"/api/post/{id}");
            if (message.IsSuccessStatusCode)
            {
                return 1;
            }
            return 0;
        }
    }
}
