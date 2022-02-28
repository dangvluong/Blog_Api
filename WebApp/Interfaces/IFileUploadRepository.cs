using WebApp.Models.Response;

namespace WebApp.Interfaces
{
    public interface IFileUploadRepository
    {
        Task<ResponseModel> UploadImage(HttpContent content, string token);
        Task<ResponseModel> UploadThumbnail(HttpContent content, string token);
    }
}
