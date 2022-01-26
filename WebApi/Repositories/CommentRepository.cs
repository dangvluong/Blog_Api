using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }

        public void AddComment(Comment comment)
        {
            Add(comment);
        }

        public void DeleteComment(Comment comment)
        {
            Delete(comment);
        }
        public async Task<Comment> GetCommentById(int id, bool trackChanges)
        {            
            return await FindByCondition(comment => comment.Id == id, trackChanges).SingleOrDefaultAsync();
        }                
        public async Task<IEnumerable<Comment>> GetCommentsByPost(int postId, bool trackChanges)
        {
            return await FindByCondition(c => c.PostId == postId, trackChanges).Include(comment => comment.Author).OrderByDescending(c => c.DateCreate).ToListAsync();
        }
        public async Task<IEnumerable<Comment>> GetCommentsByMember(int memberId, bool trackChanges)
        {
            return await FindByCondition(c => c.AuthorId == memberId, trackChanges).Include(comment => comment.Author).Include(c => c.Post).OrderByDescending(c => c.DateCreate).ToListAsync();
        }

        public void UpdateComment(Comment comment)
        {
            Update(comment);
        }

        public async Task<IEnumerable<Comment>> GetComments()
        {
            return await FindAll(trackChanges: false).Include(comment => comment.Author).Include(c=>c.Post).OrderByDescending(c => c.DateCreate).ToListAsync();
        }
    }
}
