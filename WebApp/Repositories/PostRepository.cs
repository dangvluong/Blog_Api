﻿using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        public PostRepository(HttpClient client) : base(client)
        {
        }

        public async Task<ListPostDto> GetPosts(int page)
        {
            return await Get<ListPostDto>($"/api/post?page={page}");
        }

        public async Task<Post> GetPostById(int id, bool countView = false)
        {
            string countViewParam = string.Empty;
            if (countView)
                countViewParam = "?countView=true";
            return await Get<Post>($"/api/post/{id}{countViewParam}");
        }

        public async Task<int> Create(Post post, string token)
        {
            return await PostJson<Post>("/api/post", post, token);            
        }
        public async Task<int> Edit(Post post, string token)
        {
            return await Put<Post>("/api/post", post, token);            
        }
        public async Task<int> Delete(int id, string token)
        {
            return await Delete($"/api/post/{id}", token);            
        }
        public async Task<List<Post>> GetPostsByMember(int id)
        {
            return await Get<List<Post>>($"/api/post/getpostsbymember/{id}");            
        }

        public async Task<IEnumerable<Post>> GetTrendingPost()
        {
            return await Get<IEnumerable<Post>>("/api/post/gettrendingpost");
        }

        public async Task<IEnumerable<Post>> GetMostRecentPosts()
        {
            return await Get<IEnumerable<Post>>("/api/post/getmostrecentposts");
        }

        public async Task<IEnumerable<Post>> GetTodayHighlightPosts()
        {
            return await Get<IEnumerable<Post>>("/api/post/gettodayhighlightposts");
        }

        public async Task<List<Post>> GetFeaturedPosts()
        {
            return await Get<List<Post>>("/api/post/getfeaturedposts");
        }
    }
}
