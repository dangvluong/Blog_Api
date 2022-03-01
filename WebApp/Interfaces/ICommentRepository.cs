using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetComments(string token);
        Task<List<Comment>> GetCommentsByPostId(int id);
        Task<Comment> GetComment(int id);
        Task<ResponseModel> PostComment(Comment comment, string token);
        Task<ResponseModel> EditComment(Comment comment, string token);
        Task<ResponseModel> DeleteComment(int id, string token);
        Task<List<Comment>> GetCommentsByMember(int id);
        Task<ResponseModel> UpdateComment(Comment comment, string accessToken);
    }
}
