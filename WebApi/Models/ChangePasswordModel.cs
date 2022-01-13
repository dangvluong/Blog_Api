using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [StringLength(64, MinimumLength = 6)]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6)]
        public string NewPassword { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6)]
        [Compare("NewPassword",ErrorMessage ="Mật khẩu không khớp. Hãy nhập mật khẩu mới giống nhau.")]
        public string ConfirmNewPassword { get; set; }
    }
}
