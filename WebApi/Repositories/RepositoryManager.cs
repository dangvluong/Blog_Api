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
        private PostRepository post;
        private CategoryRepository category;
        private CommentRepository comment;
        private RoleRepository role;
        private MemberRepository member;
        public MemberRepository Member
        {
            get
            {
                if (member == null)
                    member = new MemberRepository(_context);
                return member;
            }
        }
        public RoleRepository Role
        {
            get
            {
                if(role == null)
                    role = new RoleRepository(_context);
                return role;
            }
        }
        public CommentRepository Comment
        {
            get
            {
                if (comment == null)
                    comment = new CommentRepository(_context);
                return comment;
            }
        }

        public CategoryRepository Category
        {
            get
            {
                if(category == null)
                    category = new CategoryRepository(_context);
                return category;
            }
        }

        public PostRepository Post
        {
            get
            {
                if (post == null)
                    post = new PostRepository(_context);
                return post;
            }
        }


    }
    public interface IRepositoryManager
    {
        Task<int> SaveChanges();
    }
}
