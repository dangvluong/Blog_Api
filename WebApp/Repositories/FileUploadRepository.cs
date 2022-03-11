using WebApp.Interfaces;
using WebApp.Models.Response;

namespace WebApp.Repositories
{
    public class FileUploadRepository : BaseRepository, IFileUploadRepository
    {
        public FileUploadRepository(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseModel> UploadImage(HttpContent obj, string token)
        {
            return await Send<HttpContent, string>("/api/fileupload/postimage", obj, (client, url, obj) => client.PostAsync(url, obj), message => message.Content.ReadAsStringAsync(), token);
        }
        public async Task<ResponseModel> UploadThumbnail(HttpContent obj, string token)
        {
            return await Send<HttpContent, string>("/api/fileupload/postthumbnail", obj, (client, url, obj) => client.PostAsync(url, obj), message => message.Content.ReadAsStringAsync(), token);
        }
    }
}
