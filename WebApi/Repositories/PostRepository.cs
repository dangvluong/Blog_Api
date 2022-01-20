using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly int size = 10;
        public PostRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Post>> GetPosts(int page)
        {
            return await FindAll(trackChanges:false).OrderBy(p=> p.Id).Skip(size*(page-1)).Take(size).ToListAsync();
        }
        public async Task<int> CountTotalPage()
        {
            return (int)Math.Floor(await FindAll(trackChanges: false).CountAsync()/(float)size);
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
