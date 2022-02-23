using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly AppDbContext _context;

        public RepositoryManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }

        //Declare properties
        private IPostRepository post;
        private ICategoryRepository category;
        private ICommentRepository comment;
        private IRoleRepository role;
        private IMemberRepository member;
        private IRefreshTokenRepository refreshToken;
        public IRefreshTokenRepository RefreshToken
        {
            get
            {
                if (refreshToken == null)
                    refreshToken = new RefreshTokenRepository(_context);
                return refreshToken;
            }
        }
        public IMemberRepository Member
        {
            get
            {
                if (member == null)
                    member = new MemberRepository(_context);
                return member;
            }
        }
        public IRoleRepository Role
        {
            get
            {
                if(role == null)
                    role = new RoleRepository(_context);
                return role;
            }
        }
        public ICommentRepository Comment
        {
            get
            {
                if (comment == null)
                    comment = new CommentRepository(_context);
                return comment;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
                if(category == null)
                    category = new CategoryRepository(_context);
                return category;
            }
        }

        public IPostRepository Post
        {
            get
            {
                if (post == null)
                    post = new PostRepository(_context);
                return post;
            }
        }

        
    }   
}
