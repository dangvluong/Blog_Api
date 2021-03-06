namespace WebApi.Interfaces
{
    public interface IRepositoryManager
    {
        public ICategoryRepository Category{ get;}
        public ICommentRepository Comment { get; }
        public IMemberRepository Member { get; }
        public IPostRepository Post { get; }
        public IRoleRepository Role { get; }
        public IRefreshTokenRepository RefreshToken { get; }

        Task<int> SaveChanges();

    }
}
