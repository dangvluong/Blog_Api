using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Bạn phải nhập {0}")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        [Display(Name = "Địa chỉ email của tài khoản:")]
        public string Email { get; set; }
    }
}
