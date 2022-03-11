using WebApp.Models;

namespace WebApp.Interfaces
{
    public interface IStatisticRepository
    {
        Task<Statistic> GetStatistic(string token);
    }
}
