using WebApp.DataTransferObject;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Interfaces
{
    public interface IPostRepository
    {
        Task<ListPostDto> GetPosts(int page);
        Task<ListPostDto> ManagerGetPosts(int page, string token);
        Task<Post> GetPostById(int id, bool countView= false);
        Task<ResponseModel> Create(Post post, string token);
        Task<ResponseModel> Edit(Post post, string token);
        Task<ResponseModel> Delete(int id, string token);
        Task<List<Post>> GetPostsByMember(int id,string token=null);
        Task<IEnumerable<Post>> GetTrendingPost();
        Task<IEnumerable<Post>> GetMostRecentPosts();
        Task<IEnumerable<Post>> GetTodayHighlightPosts();
        Task<List<Post>> GetFeaturedPosts();
        Task<ResponseModel> Approve(int postId, string token);
        Task<ListPostDto> SearchPost(string keyword,int id);
    }
}
