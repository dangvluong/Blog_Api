using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(HttpClient client) : base(client)
        {
        }
        public async Task<List<Comment>> GetComments(string token)
        {
            ResponseModel response = await Send<List<Comment>>("/api/comment",
                (client, url) => client.GetAsync(url),
                message => message.Content.ReadAsAsync<List<Comment>>(),
                token);
            if (response is SuccessResponseModel)
                return (List<Comment>)response.Data;
            return null;               
        }
        public async Task<List<Comment>> GetCommentsByPostId(int id)
        {
            ResponseModel response = await Send<List<Comment>>($"/api/comment/GetCommentsByPost/{id}",
                (client, url) => client.GetAsync(url),
                message => message.Content.ReadAsAsync<List<Comment>>()
                );
            if (response is SuccessResponseModel)
                return (List<Comment>)response.Data;
            return null;                      
        }

        public async Task<Comment> GetComment(int id)
        {          
            ResponseModel response = await Send<Comment>($"/api/comment/{id}",
                (client, url) => client.GetAsync(url),
                message => message.Content.ReadAsAsync<Comment>()
                );
            if (response is SuccessResponseModel)
                return (Comment)response.Data;
            return null;
        }      
        public async Task<ResponseModel> PostComment(Comment obj, string token)
        {            
            return await Send<Comment>("/api/comment", obj,
                (client, url, obj) => client.PostAsJsonAsync<Comment>(url, obj),
                token);
        }
        public async Task<ResponseModel> EditComment(Comment obj, string token)
        {           
            return await Send<Comment>($"/api/comment/{obj.Id}", obj,
                (client, url, obj) => client.PutAsJsonAsync<Comment>(url, obj),
                token);
        }
        public async Task<ResponseModel> DeleteComment(int id, string token)
        {            
            return await Send($"/api/comment/{id}",(client, url) => client.DeleteAsync(url),token);
        }

        public async Task<List<Comment>> GetCommentsByMember(int id)
        {            
            ResponseModel response = await Send<List<Comment>>($"/api/comment/getcommentsbymember/{id}",
                (client, url) => client.GetAsync(url),
                message => message.Content.ReadAsAsync<List<Comment>>()
                );
            if (response is SuccessResponseModel)
                return (List<Comment>)response.Data;
            return null;
        }        
    }
}
