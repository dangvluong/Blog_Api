namespace WebApp.Interfaces
{
    public interface IFileUploadRepository
    {
        Task<string> Upload(HttpContent content, string url);
    }
}
