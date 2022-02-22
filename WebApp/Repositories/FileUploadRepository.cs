using WebApp.Interfaces;

namespace WebApp.Repositories
{
    public class FileUploadRepository : BaseRepository, IFileUploadRepository
    {
        public FileUploadRepository(HttpClient client) : base(client)
        {
        }

        public async Task<string> Upload(HttpContent content, string url)
        {
            return await PostImage(url, content);
        }
    }
}
