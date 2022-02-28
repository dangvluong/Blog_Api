﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi.DataTransferObject;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Post>> GetPosts(int page, int pageSize, bool trackChanges, bool isManager = false)
        {
            var posts = FindAll(trackChanges);
            if (!isManager)
                posts = posts.Where(p => p.IsActive == true && p.IsDeleted == false);
            return await posts.Include(p => p.Author).Include(p => p.Category).OrderBy(p => p.Id).Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();
        }

        public async Task<Post> GetPostById(int id, bool trackChanges, bool countView = false)
        {
            //if countView == true, enable trackChange, otherwise keep disable            
            Post post = await FindByCondition(p => p.Id == id,trackChanges).Include(p => p.Author).ThenInclude(m => m.Roles).Include(p => p.Category).FirstOrDefaultAsync();
            if (countView && trackChanges)
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

        public async Task<IEnumerable<Post>> GetPostsByMember(int memberId, bool trackChanges, bool includeInactivePosts = false)
        {
            IQueryable<Post> posts = FindByCondition(post => post.AuthorId == memberId && post.IsDeleted == false, trackChanges);
            if (!includeInactivePosts)
                posts = posts.Where(p => p.IsActive == true);
            return await posts.Include(p => p.Author).Include(p => p.Category).OrderByDescending(post => post.DateCreated).ToListAsync();
        }

        public void DeletePost(Post post)
        {
            post.IsDeleted = true;
        }
        public void RestorePost(Post post)
        {
            post.IsDeleted = false;
        }

        public void AddRange(List<Post> posts)
        {
            _context.Posts.AddRange(posts);
        }

        public async Task<IEnumerable<Post>> GetTrendingPost(bool trackChanges = false)
        {
            return await FindAll(trackChanges).Include(post => post.Author).Include(post => post.Category).OrderByDescending(post => post.CountView).Take(10).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetMostRecentPosts(bool trackChanges = false)
        {
            return await FindAll(trackChanges).Include(post => post.Author).Include(post => post.Category).OrderByDescending(post => post.DateCreated).Take(20).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetTodayHighlightPosts(bool trackChanges = false)
        {
            //Should replay by posts have datecreated nearst from now
            return await FindAll(trackChanges).Include(p => p.Author).Include(p => p.Category).Where(p => p.DateCreated.Month == DateTime.UtcNow.Month)
                .OrderByDescending(post => post.CountView).Take(4).ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetFeaturedPosts(bool trackChanges = false)
        {
            return await FindAll(trackChanges).Include(p => p.Author).Include(p => p.Category).Where(p => p.DateCreated.Month == DateTime.UtcNow.Month)
                .OrderByDescending(post => post.CountView).Take(4).ToListAsync();
        }

        public async Task<IEnumerable<Post>> Search(string keyword, int page, int pageSize, bool trackChanges = false)
        {
            return await FindAll(trackChanges).Where(p => p.Title.ToLower().Contains(keyword.ToLower())).Include(p => p.Author).Include(p => p.Category).OrderBy(p => p.Id).Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();
        }

        public async Task<int> CountTotalPage(int pageSize, Expression<Func<Post, bool>> conditionFilter = null)
        {
            int totalPost;
            if (conditionFilter == null)
                totalPost = await FindAll(trackChanges: false).CountAsync();
            else
                totalPost = await FindByCondition(conditionFilter, trackChanges: false).CountAsync();
            return (int)Math.Ceiling(totalPost / (float)pageSize);

        }

        public async Task<IEnumerable<Post>> GetActivePosts(int page, int pageSize, bool trackChanges)
        {
            return await FindAll(trackChanges).Where(p => p.IsActive == true && p.IsDeleted == false).Include(p => p.Author).Include(p => p.Category).OrderBy(p => p.Id).Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();
        }
    }
}
