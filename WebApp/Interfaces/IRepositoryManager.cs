﻿namespace WebApp.Interfaces
{
    public interface IRepositoryManager
    {
        public ICategoryRepository Category { get; }
        public ICommentRepository Comment { get; }
        public IAuthRepository Auth { get; }
        public IMemberRepository Member { get; }
        public IPostRepository Post { get; }
        public ISeedDataRepository SeedData { get; }
    }
}