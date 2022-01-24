namespace WebApi.Models
{
    public class ChangeAvatarModel
    {
        public int MemberId { get; set; }
        public IFormFile AvatarUpload { get; set; }
    }
}
