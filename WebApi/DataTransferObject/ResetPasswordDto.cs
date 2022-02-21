using System.ComponentModel.DataAnnotations;

namespace WebApi.DataTransferObject
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; }

        [Required]        
        public string Email { get; set; }
        [Required]       
        [StringLength(64, MinimumLength = 6, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự.")]
        public string NewPassword { get; set; }
        [Required]      
        [StringLength(64, MinimumLength = 6, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự.")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới không trùng nhau")]
        public string ConfirmNewPassword { get; set; }
    }
}
