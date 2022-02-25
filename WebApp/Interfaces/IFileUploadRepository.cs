using WebApp.Models.Response;

namespace WebApp.Interfaces
{
    public interface IFileUploadRepository
    {
        Task<ResponseModel> Upload(HttpContent content, string url, string token);
    }
}
