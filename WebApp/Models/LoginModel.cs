using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(32, MinimumLength = 6)]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "Ghi nhớ tài khoản")]
        public bool Remember { get; set; }
    }
}
