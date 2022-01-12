namespace WebApp.Interfaces
{
    public interface ISeedDataRepository
    {
        Task<int> ClearAll(); 
        Task<int> SeedData();
    }
}
