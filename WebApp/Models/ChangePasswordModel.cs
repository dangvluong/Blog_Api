using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [Display(Name = "Mật khẩu cũ")]
        [StringLength(64, MinimumLength = 6,ErrorMessage ="{0} phải dài từ {2} đến {1} ký tự.")]
        public string OldPassword { get; set; }
        [Required]
        [Display(Name = "Mật khẩu mới")]
        [StringLength(64, MinimumLength = 6,ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự.")]
        public string NewPassword { get; set; }
        [Required]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự.")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới không trùng nhau")]
        public string ConfirmNewPassword { get; set; }
    }
}
