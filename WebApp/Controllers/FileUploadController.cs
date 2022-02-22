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
            MultipartFormDataContent content = new MultipartFormDataContent();
            if (image != null || !string.IsNullOrEmpty(image.FileName))
            {                
                content.Add(new StreamContent(image.OpenReadStream()),nameof(image),image.FileName);
            }            
            string imageUrl = await _repository.FileUpload.Image(content);
            return imageUrl;
        }
    }
}
