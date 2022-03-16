using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Repositories
{
    public class PostRepository : BaseRepository, IPostRepository
    {
        public PostRepository(HttpClient client) : base(client)
        {
        }

        public async Task<ListPostDto> GetPosts(int page)
        {            
            var response = await Send<ListPostDto>($"/api/post?page={page}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<ListPostDto>());
            return (ListPostDto)response.Data;
        }
        public async Task<ListPostDto> ManagerGetPosts(int page, string token)
        {            
            var response = await Send<ListPostDto>($"/api/post/managergetposts?page={page}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<ListPostDto>(), token);
            return (ListPostDto)response.Data;
        }
        public async Task<Post> GetPostById(int id, bool countView = false)
        {
            string countViewParam = string.Empty;
            if (countView)
                countViewParam = "?countView=true";
            var response = await Send<Post>($"/api/post/{id}{countViewParam}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<Post>());
            return (Post)response.Data;          
        }

        public async Task<ResponseModel> Create(Post obj, string token)
        {
            return await Send<Post>("/api/post", obj, (client, url, obj) => client.PostAsJsonAsync<Post>(url, obj), token);           
        }
        public async Task<ResponseModel> Edit(Post obj, string token)
        {
            return await Send<Post>($"/api/post/{obj.Id}", obj, (client, url, obj) => client.PutAsJsonAsync<Post>(url, obj), token);           
        }
        public async Task<ResponseModel> Delete(int id, string token)
        {
            return await Send($"/api/post/{id}", (client, url) => client.DeleteAsync(url), token);           
        }
        public async Task<List<Post>> GetPostsByMember(int id, string token = null)
        {           
            var response = await Send<List<Post>>($"/api/post/getpostsbymember/{id}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<List<Post>>(), token);
            return (List<Post>)response.Data;
        }

        public async Task<IEnumerable<Post>> GetTrendingPost()
        {            
            var response = await Send<IEnumerable<Post>>("/api/post/gettrendingpost", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<IEnumerable<Post>>());
            return (IEnumerable<Post>)response.Data;
        }

        public async Task<IEnumerable<Post>> GetMostRecentPosts()
        {
            var response = await Send<IEnumerable<Post>>("/api/post/getmostrecentposts", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<IEnumerable<Post>>());
            return (IEnumerable<Post>)response.Data;           
        }

        public async Task<IEnumerable<Post>> GetTodayHighlightPosts()
        {
            var response = await Send<IEnumerable<Post>>("/api/post/gettodayhighlightposts", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<IEnumerable<Post>>());
            return (IEnumerable<Post>)response.Data;            
        }

        public async Task<List<Post>> GetFeaturedPosts()
        {
            var response = await Send<List<Post>>("/api/post/getfeaturedposts", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<List<Post>>());
            return (List<Post>)response.Data;            
        }

        public async Task<ResponseModel> Approve(int postId, string token)
        {            
            return await Send($"/api/post/approve/{postId}", (client, url) => client.PostAsync(url,null), token);
        }
        public async Task<ResponseModel> RemoveApproved(int postId, string token)
        {           
            return await Send($"/api/post/removeapproved/{postId}", (client, url) => client.PostAsync(url, null), token);
        }

        public async Task<ListPostDto> SearchPost(string keyword, int id)
        {
            var response = await Send<ListPostDto>($"/api/post/search?keyword={keyword}&page={id}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<ListPostDto>());
            return (ListPostDto)response.Data;           
        }

        public async Task<ResponseModel> Restore(int id, string token)
        {
            return await Send($"/api/post/restore/{id}", (client, url) => client.PostAsync(url,null), token);
        }

        public async Task<ListPostDto> GetPostsFromCategory(int categoryId, int page)
        {
            var response = await Send<ListPostDto>($"/api/post/fromcategory/{categoryId}?page={page}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<ListPostDto>());
            return (ListPostDto)response.Data;
        }

        public async Task<ListPostDto> GetNewPosts(int page,string accessToken)
        {
            var response = await Send<ListPostDto>($"/api/post/getpostswithin30days?page={page}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<ListPostDto>(),accessToken);
            return (ListPostDto)response.Data;
        }
        public async Task<ListPostDto> GetUnapprovedPosts(int page,string accessToken)
        {
            var response = await Send<ListPostDto>($"/api/post/getunapprovedposts?page={page}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<ListPostDto>(),accessToken);
            return (ListPostDto)response.Data;
        }
    }
}
