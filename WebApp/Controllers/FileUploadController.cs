using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;

namespace WebApp.Controllers
{
    public class FileUploadController : BaseController
    {
        public FileUploadController(IRepositoryManager repository) : base(repository)
        {
        }
        [HttpPost]
        public async Task<string> Image(IFormFile image)
        {            
            if (image != null || !string.IsNullOrEmpty(image.FileName))
            {
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Add(new StreamContent(image.OpenReadStream()), nameof(image), image.FileName);
                string url = "/api/fileupload/postimage";
                string imageUrl = await _repository.FileUpload.Upload(content,url);
                return imageUrl;                
            }
            return null;
        }
    }
}
