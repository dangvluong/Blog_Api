using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class CommentRepository : BaseRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IList<Comment>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }
        public async Task<Comment> GetComment(int id)
        {
            return await _context.Comments.FindAsync(id);
        }
        public async Task<IEnumerable<Comment>> GetCommentsByPost(int postId)
        {
            return await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
        }
        public void Update(Comment comment)
        {
            _context.Comments.Update(comment);
        }
        public void Add(Comment comment)
        {
            _context.Comments.Add(comment);
        }
        public void AddRange(List<Comment> comment)
        {
            _context.Comments.AddRange(comment);
        }
        public void Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
        }        
    }
}
