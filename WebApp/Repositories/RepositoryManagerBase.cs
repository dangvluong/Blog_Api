namespace WebApp.Repositories
{
    public abstract class RepositoryManagerBase : IDisposable
    {
        private HttpClient client;
        private IConfiguration configuration;
        //private Uri apiServer;
        //private Uri ApiServer
        //{
        //    get
        //    {
        //        if (apiServer == null)
        //            apiServer = new Uri(configuration.GetSection("ApiServer").Value);
        //        return apiServer;
        //    }
        //}

        
        public RepositoryManagerBase(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        protected HttpClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new HttpClient();
                    client.BaseAddress = new Uri(configuration.GetSection("ApiServer").Value);
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
