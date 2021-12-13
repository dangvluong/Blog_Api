using WebApp.Models;

namespace WebApp.Helper
{
    public class SiteProvider : IDisposable
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
                if (category is null)
                {
                    category = new CategoryRepository(Client);
                }
                return category;
            }

        }
        private PostRepository post;

        public PostRepository Post
        {
            get
            {
                if (post is null)
                    post = new PostRepository(Client);
                return post;
            }

        }
        private MemberRepository member;

        public MemberRepository Member
        {
            get
            {
                if (member is null)
                    member = new MemberRepository(Client);
                return member;
            }

        }

        private SeedDataRepository seedData;

        public SeedDataRepository SeedData
        {
            get
            {
                return seedData ?? new SeedDataRepository(Client);                
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
