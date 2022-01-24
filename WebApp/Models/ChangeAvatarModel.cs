using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ChangeAvatarModel
    {        
        public int MemberId { get; set; }     
        [Required]
        [Display(Name ="Chọn hình ảnh đại diện: ")]
        public IFormFile AvatarUpload { get; set; }
    }
}
