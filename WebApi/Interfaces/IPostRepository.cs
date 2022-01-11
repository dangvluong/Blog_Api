using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> GetPost(int id);
        Task<IEnumerable<Post>> GetPosts();
        Task<IEnumerable<Post>> GetPostsByMember(int memberId);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void DeletePost(Post post);
    }
}
