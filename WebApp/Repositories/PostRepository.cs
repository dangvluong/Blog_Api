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
            //return await Get<ListPostDto>($"/api/post?page={page}");
            var response = await Send<ListPostDto>($"/api/post?page={page}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<ListPostDto>());
            return (ListPostDto)response.Data;
        }
        public async Task<ListPostDto> ManagerGetPosts(int page, string token)
        {
            //return await Get<ListPostDto>($"/api/post/managergetposts?page={page}", token);
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
            //return await Get<Post>($"/api/post/{id}{countViewParam}");
        }

        public async Task<ResponseModel> Create(Post obj, string token)
        {
            return await Send<Post>("/api/post", obj, (client, url, obj) => client.PostAsJsonAsync<Post>(url, obj), token);
            //return await PostJson<Post,int>("/api/post", post, token);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //HttpResponseMessage message = await client.PostAsJsonAsync<Post>("/api/post", obj);
            //if (message.IsSuccessStatusCode)
            //{
            //    return new SuccessResponseModel
            //    {
            //        Status = (int)message.StatusCode
            //    };
            //}
            //try
            //{
            //    return JsonConvert.DeserializeObject<ErrorValidationResponseModel>(await message.Content.ReadAsStringAsync());
            //}
            //catch (Exception)
            //{
            //    return new ErrorMessageResponseModel
            //    {
            //        Status = (int)message.StatusCode,
            //        Data = await message.Content.ReadAsStringAsync()
            //    };
            //}
        }
        public async Task<ResponseModel> Edit(Post obj, string token)
        {
            return await Send<Post>("/api/post", obj, (client, url, obj) => client.PutAsJsonAsync<Post>(url, obj), token);
            //return await Put<Post>("/api/post", post, token);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //HttpResponseMessage message = await client.PutAsJsonAsync<Post>("/api/post", obj);
            //if (message.IsSuccessStatusCode)
            //{
            //    return new SuccessResponseModel
            //    {
            //        Status = (int)message.StatusCode
            //    };
            //}
            //try
            //{
            //    return JsonConvert.DeserializeObject<ErrorValidationResponseModel>(await message.Content.ReadAsStringAsync());
            //}
            //catch (Exception)
            //{
            //    return new ErrorMessageResponseModel
            //    {
            //        Status = (int)message.StatusCode,
            //        Data = await message.Content.ReadAsStringAsync()
            //    };
            //}
        }
        public async Task<ResponseModel> Delete(int id, string token)
        {
            return await Send($"/api/post/{id}", (client, url) => client.DeleteAsync(url), token);
            //return await Delete($"/api/post/{id}", token);            
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //HttpResponseMessage message = await client.DeleteAsync($"/api/post/{id}");
            //if (message.IsSuccessStatusCode)
            //{
            //    return new SuccessResponseModel
            //    {
            //        Status = (int)message.StatusCode
            //    };
            //}
            //try
            //{
            //    return JsonConvert.DeserializeObject<ErrorValidationResponseModel>(await message.Content.ReadAsStringAsync());
            //}
            //catch (Exception)
            //{
            //    return new ErrorMessageResponseModel
            //    {
            //        Status = (int)message.StatusCode,
            //        Data = await message.Content.ReadAsStringAsync()
            //    };
            //}
        }
        public async Task<List<Post>> GetPostsByMember(int id, string token = null)
        {
            //return await Get<List<Post>>($"/api/post/getpostsbymember/{id}",token);            

            var response = await Send<List<Post>>($"/api/post/getpostsbymember/{id}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<List<Post>>(), token);
            return (List<Post>)response.Data;
        }

        public async Task<IEnumerable<Post>> GetTrendingPost()
        {
            //return await Get<IEnumerable<Post>>("/api/post/gettrendingpost");
            var response = await Send<IEnumerable<Post>>("/api/post/gettrendingpost", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<IEnumerable<Post>>());
            return (IEnumerable<Post>)response.Data;
        }

        public async Task<IEnumerable<Post>> GetMostRecentPosts()
        {
            var response = await Send<IEnumerable<Post>>("/api/post/getmostrecentposts", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<IEnumerable<Post>>());
            return (IEnumerable<Post>)response.Data;
            //return await Get<IEnumerable<Post>>("/api/post/getmostrecentposts");
        }

        public async Task<IEnumerable<Post>> GetTodayHighlightPosts()
        {
            var response = await Send<IEnumerable<Post>>("/api/post/gettodayhighlightposts", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<IEnumerable<Post>>());
            return (IEnumerable<Post>)response.Data;
            //return await Get<IEnumerable<Post>>("/api/post/gettodayhighlightposts");
        }

        public async Task<List<Post>> GetFeaturedPosts()
        {
            var response = await Send<List<Post>>("/api/post/getfeaturedposts", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<List<Post>>());
            return (List<Post>)response.Data;
            //return await Get<List<Post>>("/api/post/getfeaturedposts");
        }

        public async Task<ResponseModel> Approve(int postId, string token)
        {
            //return await Post<int>($"/api/post/approve/{postId}", token: token);
            return await Send($"/api/post/approve/{postId}", (client, url) => client.PostAsync(url,null), token);
        }
        public async Task<ResponseModel> RemoveApproved(int postId, string token)
        {
            //return await Post<int>($"/api/post/approve/{postId}", token: token);
            return await Send($"/api/post/removeapproved/{postId}", (client, url) => client.PostAsync(url, null), token);
        }

        public async Task<ListPostDto> SearchPost(string keyword, int id)
        {
            var response = await Send<ListPostDto>($"/api/post/search?keyword={keyword}&page={id}", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<ListPostDto>());
            return (ListPostDto)response.Data;
            //return await Get<ListPostDto>($"/api/post/search?keyword={keyword}&page={id}");
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
    }
}
