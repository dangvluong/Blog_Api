namespace WebApp.Models
{
    public class BaseRepository
    {
        protected readonly Uri ApiServer = new Uri("https://localhost:7207/");
        protected HttpClient _client;
        public BaseRepository(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = ApiServer;
        }
    }
}
