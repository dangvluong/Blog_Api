namespace WebApp.Models
{
    public class BaseRepository
    {
        protected readonly Uri ApiServer = new Uri("https://localhost:7207/");
        protected HttpClient client;
        public BaseRepository(HttpClient client)
        {
            this.client = client;
            if (client.BaseAddress is null)
                client.BaseAddress = ApiServer;            
        }
    }
}
