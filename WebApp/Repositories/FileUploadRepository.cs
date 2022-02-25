using WebApp.Interfaces;
using WebApp.Models.Response;

namespace WebApp.Repositories
{
    public class FileUploadRepository : BaseRepository, IFileUploadRepository
    {
        public FileUploadRepository(HttpClient client) : base(client)
        {
        }

        public async Task<ResponseModel> Upload(HttpContent obj, string url, string token)
        {
            //return await PostImage(url, content);
            return await Send<HttpContent, string>(url, obj, (client, url, obj) => client.PostAsync(url, obj), message => message.Content.ReadAsStringAsync(), token);
            //var response = await Send<string>(url)
        }
    }
}
