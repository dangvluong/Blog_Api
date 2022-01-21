using Microsoft.EntityFrameworkCore;
using WebApi.DataTransferObject;
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
            return await FindAll(trackChanges: false).OrderBy(p => p.Id).Skip(size * (page - 1)).Take(size).ToListAsync();
        }
        public async Task<int> CountTotalPage()
        {
            return (int)Math.Floor(await FindAll(trackChanges: false).CountAsync() / (float)size);
        }
        public async Task<Post> GetPost(int id, bool countView = false)
        {
            //if countView == true, enable trackChange, otherwise keep disable
            Post post = await FindByCondition(p => p.Id == id, trackChanges: countView).FirstOrDefaultAsync();
            if (countView)
            {
                post.CountView++;
                _context.SaveChanges();
            }
            //return await _context.Posts.FindAsync(id);
            return post;
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

        public void AddRange(List<Post> posts)
        {
            _context.Posts.AddRange(posts);
        }

        public async Task<IEnumerable<PostDto>> GetTrendingPost()
        {
            return await FindAll(trackChanges: false).Include(post => post.Author).Include(post => post.Category).OrderByDescending(post => post.CountView).Take(10).Select(post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                DateCreated = post.DateCreated,
                DateModifier = post.DateModifier,
                IsActive = post.IsActive,
                IsDeleted = post.IsDeleted,
                CategoryId = post.CategoryId,
                Category = post.Category,
                AuthorId = post.AuthorId,
                Author = new MemberDto
                {
                    Id = post.Author.Id,
                    Username = post.Author.Username,
                    Gender = post.Author.Gender,
                    FullName = post.Author.FullName,
                    Email = post.Author.Email,
                    DateCreate = post.Author.DateCreate,
                    DateOfBirth = post.Author.DateOfBirth,
                    IsActive = post.Author.IsActive,
                    IsBanned = post.Author.IsBanned
                },
                Comments = post.Comments,
                CountView = post.CountView

            }).ToListAsync();
        }

        public async Task<IEnumerable<PostDto>> GetMostRecentPosts()
        {
            return await FindAll(trackChanges: false).Include(post => post.Author).Include(post => post.Category).OrderByDescending(post => post.DateCreated).Take(20).Select(post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                DateCreated = post.DateCreated,
                DateModifier = post.DateModifier,
                IsActive = post.IsActive,
                IsDeleted = post.IsDeleted,
                CategoryId = post.CategoryId,
                Category = post.Category,
                AuthorId = post.AuthorId,
                Author = new MemberDto
                {
                    Id = post.Author.Id,
                    Username = post.Author.Username,
                    Gender = post.Author.Gender,
                    FullName = post.Author.FullName,
                    Email = post.Author.Email,
                    DateCreate = post.Author.DateCreate,
                    DateOfBirth = post.Author.DateOfBirth,
                    IsActive = post.Author.IsActive,
                    IsBanned = post.Author.IsBanned
                },
                Comments = post.Comments,
                CountView = post.CountView

            }).ToListAsync();
        }

        public async Task<IEnumerable<PostDto>> GetTodayHighlightPosts()
        {
            //Should replay by posts have datecreated nearst from now
            return await FindAll(trackChanges: false).Include(p => p.Author).Include(p => p.Category).Where(p => p.DateCreated.Month == DateTime.UtcNow.Month)
                .OrderByDescending(post => post.CountView).Take(4).Select(post => new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Description = post.Description,
                    Content = post.Content,
                    DateCreated = post.DateCreated,
                    DateModifier = post.DateModifier,
                    IsActive = post.IsActive,
                    IsDeleted = post.IsDeleted,
                    CategoryId = post.CategoryId,
                    Category = post.Category,
                    AuthorId = post.AuthorId,
                    Author = new MemberDto
                    {
                        Id = post.Author.Id,
                        Username = post.Author.Username,
                        Gender = post.Author.Gender,
                        FullName = post.Author.FullName,
                        Email = post.Author.Email,
                        DateCreate = post.Author.DateCreate,
                        DateOfBirth = post.Author.DateOfBirth,
                        IsActive = post.Author.IsActive,
                        IsBanned = post.Author.IsBanned
                    },
                    Comments = post.Comments,
                    CountView = post.CountView

                }).ToListAsync();
        }

        public async Task<IEnumerable<PostDto>> GetFeaturedPosts()
        {
            return await FindAll(trackChanges: false).Include(p => p.Author).Include(p => p.Category).Where(p => p.DateCreated.Month == DateTime.UtcNow.Month)
                .OrderByDescending(post => post.CountView).Take(4).Select(post => new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Description = post.Description,
                    Content = post.Content,
                    DateCreated = post.DateCreated,
                    DateModifier = post.DateModifier,
                    IsActive = post.IsActive,
                    IsDeleted = post.IsDeleted,
                    CategoryId = post.CategoryId,
                    Category = post.Category,
                    AuthorId = post.AuthorId,
                    Author = new MemberDto
                    {
                        Id = post.Author.Id,
                        Username = post.Author.Username,
                        Gender = post.Author.Gender,
                        FullName = post.Author.FullName,
                        Email = post.Author.Email,
                        DateCreate = post.Author.DateCreate,
                        DateOfBirth = post.Author.DateOfBirth,
                        IsActive = post.Author.IsActive,
                        IsBanned = post.Author.IsBanned
                    },
                    Comments = post.Comments,
                    CountView = post.CountView

                }).ToListAsync();
        }
    }
}
