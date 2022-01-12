namespace WebApp.Repositories
{
    public abstract class RepositoryManagerBase : IDisposable
    {
        private HttpClient client;
        protected readonly Uri ApiServer = new Uri("https://localhost:7207/");
        protected HttpClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new HttpClient();
                    client.BaseAddress = ApiServer;
                }
                return client;
            }
        }
        public void Dispose()
        {
            if (client != null)
            {
                client.Dispose();
            }
        }
    }
}
