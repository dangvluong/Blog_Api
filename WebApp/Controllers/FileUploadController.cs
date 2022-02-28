using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
using WebApp.Models.Response;

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
                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {
                    content.Add(new StreamContent(image.OpenReadStream()), nameof(image), image.FileName);
                    ResponseModel response = await _repository.FileUpload.UploadImage(content, AccessToken);
                    if (response is SuccessResponseModel)
                        return (string)response.Data;
                }
            }
            return null;
        }
    }
}
