namespace WebApp.Interfaces
{
    public interface IFileUploadRepository
    {
        Task<string> Image(HttpContent content);
    }
}
