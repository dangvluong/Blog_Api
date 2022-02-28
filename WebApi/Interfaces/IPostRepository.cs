using System.Linq.Expressions;
using WebApi.DataTransferObject;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> GetPostById(int id, bool trackChanges, bool countView = false);
        //Task<IEnumerable<Post>> GetAllPosts(int page,int pageSize, bool trackChanges);
        Task<int> CountTotalPage(int pageSize,Expression<Func<Post, bool>> conditionFilter = null);
        Task<IEnumerable<Post>> GetPostsByMember(int memberId, bool trackChanges, bool includeInactivePost = false);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void DeletePost(Post post);
        void RestorePost(Post post);
        Task<IEnumerable<Post>> GetTrendingPost(bool trackChanges= false);
        Task<IEnumerable<Post>> GetPosts(int page, int pageSize, bool trackChanges, bool isManager = false);
        Task<IEnumerable<Post>> GetMostRecentPosts(bool trackChanges = false);
        Task<IEnumerable<Post>> GetTodayHighlightPosts(bool trackChanges = false);
        Task<IEnumerable<Post>> GetFeaturedPosts(bool trackChanges = false);
        Task<IEnumerable<Post>> Search(string keyword,int page, int pageSize, bool trackChanges = false);
        Task<IEnumerable<Post>> GetActivePosts(int page, int pageSize, bool trackChanges);
    }
}
