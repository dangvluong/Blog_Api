
using System.Net.Http.Headers;

namespace WebApp.Models
{
    public class PostRepository : BaseRepository
    {
        public PostRepository(HttpClient client) : base(client)
        {
        }

        public async Task<IList<Post>> GetPosts()
        {
            return await Get<IList<Post>>("/api/post");
        }

        public async Task<Post> GetPostById(int id)
        {
            return await Get<Post>($"/api/post/{id}");
        }

        public async Task<int> Create(Post post, string token)
        {
            return await Post<Post>("/api/post", post, token);            
        }
        public async Task<int> Edit(Post post, string token)
        {
            return await Put<Post>("/api/post", post, token);            
        }
        public async Task<int> Delete(int id, string token)
        {
            return await Delete($"/api/post/{id}", token);            
        }
        public async Task<IEnumerable<Post>> GetPostsByMember(int id)
        {
            return await Get<IEnumerable<Post>>($"/api/post/getpostsbymember/{id}");            
        }
    }
}
