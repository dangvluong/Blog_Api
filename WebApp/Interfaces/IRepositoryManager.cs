namespace WebApp.Interfaces
{
    public interface IRepositoryManager
    {
        public ICategoryRepository Category { get; }
        public ICommentRepository Comment { get; }
        public IAuthRepository Auth { get; }
        public IMemberRepository Member { get; }
        public IPostRepository Post { get; }
        public IRoleRepository Role { get; }        
        public IFileUploadRepository FileUpload { get; }
        public IStatisticRepository Statistic { get; }
    }
}
