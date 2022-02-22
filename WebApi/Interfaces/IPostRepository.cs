using System.Linq.Expressions;
using WebApi.DataTransferObject;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> GetPostById(int id, bool trackChanges, bool countView = false);
        Task<IEnumerable<Post>> GetPosts(int page,int pageSize, bool trackChanges);
        Task<int> CountTotalPage(int pageSize,bool trackChanges = false, Expression<Func<Post, bool>> conditionFilter = null);
        Task<IEnumerable<Post>> GetPostsByMember(int memberId, bool trackChanges);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void DeletePost(Post post);
        Task<IEnumerable<Post>> GetTrendingPost(bool trackChanges= false);
        Task<IEnumerable<Post>> GetMostRecentPosts(bool trackChanges = false);
        Task<IEnumerable<Post>> GetTodayHighlightPosts(bool trackChanges = false);
        Task<IEnumerable<Post>> GetFeaturedPosts(bool trackChanges = false);
        Task<IEnumerable<Post>> Search(string keyword,int page, int pageSize, bool trackChanges = false);
    }
}
