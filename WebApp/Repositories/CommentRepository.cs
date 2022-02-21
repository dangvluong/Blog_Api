using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(HttpClient client) : base(client)
        {
        }
        public async Task<List<Comment>> GetComments()
        {
            return await Get<List<Comment>>("/api/comment");            
        }
        public async Task<List<Comment>> GetCommentsByPostId(int id)
        {
            return await Get<List<Comment>>($"/api/comment/GetCommentsByPost/{id}");            
        }        

        public async Task<Comment> GetComment(int id)
        {
            return await Get<Comment>($"/api/comment/{id}");            
        }
        //Override more method to return comment after post (?)
        public async Task<int> PostComment(Comment comment, string token)
        {
            return await PostJson<Comment,int>("/api/comment", comment, token);            
        }
        public async Task<int> EditComment(Comment comment, string token)
        {
            return await Put<Comment>("/api/comment", comment, token);            
        }
        public async Task<int> DeleteComment(int id, string token)
        {
            return await Delete($"/api/comment/{id}", token);            
        }

        public async Task<List<Comment>> GetCommentsByMember(int id)
        {
            return await Get<List<Comment>>($"/api/comment/getcommentsbymember/{id}");
        }
    }
}
