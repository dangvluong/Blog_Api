using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetComments();
        Task<List<Comment>> GetCommentsByPostId(int id);
        Task<Comment> GetComment(int id);
        Task<int> PostComment(Comment comment, string token);
        Task<int> EditComment(Comment comment, string token);
        Task<int> DeleteComment(int id, string token);
    }
}
