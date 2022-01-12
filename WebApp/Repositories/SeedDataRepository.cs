using WebApp.Interfaces;

namespace WebApp.Repositories
{
    public class SeedDataRepository : BaseRepository, ISeedDataRepository
    {
        public SeedDataRepository(HttpClient client) : base(client)
        {
        }
        public async Task<int> ClearAll()
        {
            HttpResponseMessage message = await client.GetAsync("/api/seeddata/cleardata");
            if (message.IsSuccessStatusCode)
            {
                await client.GetAsync("/api/seeddata/migrate");
                return 1;
            }    
            return 0;
        }
        public async Task<int> SeedData()
        {
            HttpResponseMessage message = await client.GetAsync("/api/seeddata/seeddata");
            if (message.IsSuccessStatusCode)
                return 1;
            return 0;
        }
    }
}
