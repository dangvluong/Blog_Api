using WebApp.Interfaces;

namespace WebApp.Repositories
{
    public class FileUploadRepository : BaseRepository, IFileUploadRepository
    {
        public FileUploadRepository(HttpClient client) : base(client)
        {
        }

        public async Task<string> Image(HttpContent content)
        {
            return await PostImage("/api/fileupload/postimage", content);
        }
    }
}
