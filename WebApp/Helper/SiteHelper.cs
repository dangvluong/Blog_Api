using WebApp.Models;

namespace WebApp.Helper
{
    public class SiteHelper:IDisposable
    {
        private CategoryRepository category;
        //private readonly Uri ApiServer = new Uri("https://localhost:7207/");
        private HttpClient client;
        public HttpClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new HttpClient();
                }
                return client;
            }
        }
        public CategoryRepository Category
        {
            get
            {
                if (category == null)
                {
                    category = new CategoryRepository(Client);
                }
                return category;
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
