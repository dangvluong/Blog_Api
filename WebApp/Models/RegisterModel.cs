using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Bạn phải nhập {0}")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự")]
        [Display(Name = "Tên tài khoản")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập {0}")]
        [StringLength(64, MinimumLength = 6, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Giới tính")]
        public bool Gender { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập {0}")]
        [Display(Name = "Tên đầy đủ")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập {0}")]
        [EmailAddress(ErrorMessage ="Định dạng email không hợp lệ")]
        [Display(Name = "Địa chỉ email")]
        public string Email { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Bạn phải chọn {0}")]
        [Display(Name = "Ngày sinh")]
        public DateTime DateOfBirth { get; set; }        
    }
}
