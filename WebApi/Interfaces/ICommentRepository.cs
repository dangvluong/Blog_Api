using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> GetComment(int id);
        Task<IEnumerable<CommentDto>> GetCommentsByPost(int postId);
        void UpdateComment(Comment comment);
        void AddComment(Comment comment);
        void DeleteComment(Comment comment);
    }
}
