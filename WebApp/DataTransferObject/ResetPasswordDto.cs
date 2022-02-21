using System.ComponentModel.DataAnnotations;

namespace WebApp.DataTransferObject
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [Display(Name ="Địa chỉ email")]
        [EmailAddress(ErrorMessage ="Định dạng email không đúng")]
        public string Email { get; set; }
        [Required]        
        [Display(Name = "Mật khẩu mới")]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự.")]
        public string NewPassword { get; set; }
        [Required]        
        [Display(Name = "Nhập lại mật khẩu mới")]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự.")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới không trùng nhau")]
        public string ConfirmNewPassword { get; set; }
    }
}
