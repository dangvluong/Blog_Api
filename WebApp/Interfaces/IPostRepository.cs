using WebApp.DataTransferObject;
using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IPostRepository
    {
        Task<ListPostDto> GetPosts(int page);
        Task<ListPostDto> ManagerGetPosts(int page, string token);
        Task<Post> GetPostById(int id, bool countView= false);
        Task<int> Create(Post post, string token);
        Task<int> Edit(Post post, string token);
        Task<int> Delete(int id, string token);
        Task<List<Post>> GetPostsByMember(int id,string token=null);
        Task<IEnumerable<Post>> GetTrendingPost();
        Task<IEnumerable<Post>> GetMostRecentPosts();
        Task<IEnumerable<Post>> GetTodayHighlightPosts();
        Task<List<Post>> GetFeaturedPosts();
        Task<int> Approve(int postId, string token);
        Task<ListPostDto> SearchPost(string keyword,int id);
    }
}
