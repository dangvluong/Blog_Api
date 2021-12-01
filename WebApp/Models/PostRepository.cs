
namespace WebApp.Models
{
    public class PostRepository
    {
        public async Task<List<Post>> GetPosts()
        {
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7207/");
                HttpResponseMessage message = await client.GetAsync("/api/post");
                if (message.IsSuccessStatusCode)
                {
                    return await message.Content.ReadAsAsync<List<Post>>();
                }
                return new List<Post>();
            };
        }
    }
}
