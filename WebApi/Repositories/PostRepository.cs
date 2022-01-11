using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await FindAll(trackChanges:false).ToListAsync();
        }
        public async Task<Post> GetPost(int id)
        {
            //return await _context.Posts.FindAsync(id);
            return await FindByCondition(p => p.Id == id,false).FirstOrDefaultAsync();
        }
        public void UpdatePost(Post post)
        {
            Update(post);
        }
        public void AddPost(Post post)
        {
            Add(post);
        }
      
        public async Task<IEnumerable<Post>> GetPostsByMember(int memberId)
        {
            //return await _context.Posts.Where(p => p.AuthorId == memberId && p.IsDeleted == false).OrderByDescending(p => p.DateCreated).ToListAsync();
            return await FindByCondition(post => post.AuthorId == memberId && post.IsDeleted == false, false)
                .OrderByDescending(post => post.DateCreated)
                .ToListAsync();
        }      

        public void DeletePost(Post post)
        {
            post.IsDeleted = true;
        }

        #region Only for seed data
        public void AddRange(List<Post> posts)
        {
            _context.Posts.AddRange(posts);
        }
        #endregion
    }
}
