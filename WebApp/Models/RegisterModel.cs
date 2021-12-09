using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(32, MinimumLength = 6)]
        [Display(Name = "Tên tài khoản")]
        public string Username { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Giới tính")]
        public bool Gender { get; set; }
        [Required]
        [Display(Name = "Tên đầy đủ")]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Địa chỉ email")]
        public string Email { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime DateOfBirth { get; set; }        
    }
}
