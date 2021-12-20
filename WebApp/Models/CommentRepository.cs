namespace WebApp.Models
{
    public class CommentRepository : BaseRepository
    {
        public CommentRepository(HttpClient client) : base(client)
        {
        }
        public async Task<IList<Comment>> GetComments()
        {
            HttpResponseMessage message = await client.GetAsync("/api/comment");
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<IList<Comment>>();
            }
            return null;
        }
        public async Task<List<Comment>> GetCommentByPostId(int id)
        {
            HttpResponseMessage message = await client.GetAsync($"/api/comment/GetCommentsByPost/{id}");
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<List<Comment>>();
            }
            return null;
        }        

        public async Task<Comment> GetComment(int id)
        {
            HttpResponseMessage message = await client.GetAsync($"/api/comment/{id}");
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<Comment>();
            }
            return null;
        }
        public async Task<Comment> PostComment(Comment comment)
        {
            HttpResponseMessage message = await client.PostAsJsonAsync<Comment>("/api/comment",comment);
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<Comment>();
            }
            return null;
        }
        public async Task<Comment> EditComment(Comment comment)
        {
            HttpResponseMessage message = await client.PutAsJsonAsync<Comment>("/api/comment", comment);
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<Comment>();
            }
            return null;
        }
        public async Task<int> DeleteComment(int id)
        {
            HttpResponseMessage message = await client.DeleteAsync($"/api/comment/{id}");
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<int>();
            }
            return 0;
        }
    }
}
