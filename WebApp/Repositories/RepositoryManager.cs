﻿using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class RepositoryManager : RepositoryManagerBase, IRepositoryManager
    {        
        private ICategoryRepository category;       
        private IPostRepository post;
        private IMemberRepository member;
        private ICommentRepository comment;
        private IAuthRepository auth;
        private ISeedDataRepository seedData;
       
        public ICategoryRepository Category
        {
            get
            {
                if (category is null)
                {
                    category = new CategoryRepository(Client);
                }
                return category;
            }

        }        

        public IPostRepository Post
        {
            get
            {
                if (post is null)
                    post = new PostRepository(Client);
                return post;
            }
        }       

        public IMemberRepository Member
        {
            get
            {
                if (member is null)
                    member = new MemberRepository(Client);
                return member;
            }
        }       

        public ISeedDataRepository SeedData
        {
            get
            {
                if (seedData is null)
                    seedData = new SeedDataRepository(Client);
                return seedData;
            }
        }        

        public ICommentRepository Comment
        {
            get
            {
                if (comment is null)
                    comment = new CommentRepository(Client);
                return comment;
            }
        }
        
        public IAuthRepository Auth
        {
            get
            {
                if (auth == null)
                    auth = new AuthRepository(Client);
                return auth;
            }
        }        
    }
}