using System.Linq.Expressions;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetComments();
        Task<Comment> GetCommentById(int id, bool trackChanges);        
        Task<IEnumerable<Comment>> GetCommentsByPost(int postId, bool trackChanges);
        Task<IEnumerable<Comment>> GetCommentsByMember(int memberId, bool trackChanges);
        void UpdateComment(Comment comment);
        void AddComment(Comment comment);
        void DeleteComment(Comment comment);
    }
}
