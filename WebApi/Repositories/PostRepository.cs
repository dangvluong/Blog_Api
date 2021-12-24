using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class PostRepository : BaseRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IList<Post>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }
        public async Task<Post> GetPost(int id)
        {
            return await _context.Posts.FindAsync(id);
        }
        public void Update(Post post)
        {
            _context.Posts.Update(post);
        }
        public void Add(Post post)
        {
            _context.Posts.Add(post);
        }
        public void AddRange(List<Post> posts)
        {
            _context.Posts.AddRange(posts);
        }
        public async Task<IList<Post>> GetPostsByMember(int memberId)
        {
            return await _context.Posts.Where(p => p.AuthorId == memberId && p.IsDeleted == false).OrderByDescending(p => p.DateCreated).ToListAsync();
        }       
    }
}
