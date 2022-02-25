using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ChangeAvatarModel
    {        
        [Required]
        public int MemberId { get; set; }     
        [Required(ErrorMessage ="Bạn phải chọn hình đại diện")]
        [Display(Name ="Chọn hình ảnh đại diện: ")]
        public IFormFile AvatarUpload { get; set; }
    }
}
