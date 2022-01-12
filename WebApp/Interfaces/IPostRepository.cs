using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetPosts();
        Task<Post> GetPostById(int id);
        Task<int> Create(Post post, string token);
        Task<int> Edit(Post post, string token);
        Task<int> Delete(int id, string token);
        Task<IEnumerable<Post>> GetPostsByMember(int id);
    }
}
