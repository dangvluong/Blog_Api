using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Enums;
using WebApi.Helper;
using WebApi.Interfaces;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : BaseController
    {
        public FileUploadController(IRepositoryManager repository) : base(repository)
        {
        }
        [Authorize]
        [HttpPost("postimage")]
        public ActionResult<string> PostImage([FromForm] IFormFile image)
        {
            if (image == null || string.IsNullOrEmpty(image.FileName))
                return BadRequest();
            string imageUrl = SiteHelper.UploadFile(image, UploadTypes.PostImages);            
            return Ok(imageUrl);
        }
        [Authorize]
        [HttpPost("postthumbnail")]
        public ActionResult<string> PostThumbnail([FromForm] IFormFile thumbnailImage)
        {
            if (thumbnailImage == null || string.IsNullOrEmpty(thumbnailImage.FileName))
                return BadRequest();
            string imageUrl = SiteHelper.UploadFile(thumbnailImage, UploadTypes.PostThumbnail);
            return Ok(imageUrl);
        }
    }
}