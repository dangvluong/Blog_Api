namespace WebApp.Models
{
    public class CommentRepository : BaseRepository
    {
        public CommentRepository(HttpClient client) : base(client)
        {
        }
        public async Task<IList<Comment>> GetComments()
        {
            return await Get<IList<Comment>>("/api/comment");            
        }
        public async Task<List<Comment>> GetCommentByPostId(int id)
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
            return await Post<Comment>("/api/comment", comment, token);            
        }
        public async Task<int> EditComment(Comment comment, string token)
        {
            return await Put<Comment>("/api/comment", comment, token);            
        }
        public async Task<int> DeleteComment(int id, string token)
        {
            return await Delete($"/api/comment/{id}", token);            
        }
    }
}
