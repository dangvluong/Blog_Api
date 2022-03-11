using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Repositories
{
    public class StatisticRepository : BaseRepository, IStatisticRepository
    {
        public StatisticRepository(HttpClient client) : base(client)
        {
        }

        public async Task<Statistic> GetStatistic(string token)
        {
            ResponseModel response = await Send<Statistic>("/api/statistic", (client, url) => client.GetAsync(url), message => message.Content.ReadAsAsync<Statistic>(), token);
            if(response is SuccessResponseModel)
                return (Statistic)response.Data;
            return null;
        }
    }
}
