using Microsoft.EntityFrameworkCore;
using WebApi.Dto;
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
       
        public async Task<Comment> GetComment(int id)
        {
            //return await _context.Comments.FindAsync(id);
            return await FindByCondition(comment => comment.Id == id, false).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<CommentDto>> GetCommentsByPost(int postId)
        {
            //return await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
            return await FindByCondition(comment => comment.PostId == postId, trackChanges: false).Include(comment => comment.Author).Select(comment => new CommentDto
            {
                Id= comment.Id,
                Content= comment.Content,
                AuthorId = comment.AuthorId,
                AuthorName = comment.Author.FullName,
                PostId = comment.PostId,                
                DateCreate = comment.DateCreate                
            }).OrderByDescending(c => c.DateCreate).ToListAsync();
        }

        public void UpdateComment(Comment comment)
        {
            Update(comment);
        }
     
    }
}
