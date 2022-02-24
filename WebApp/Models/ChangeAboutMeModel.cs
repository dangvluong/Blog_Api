using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ChangeAboutMeModel
    {        
        public int MemberId { get; set; }
        [Required(ErrorMessage ="Bạn phải nhập {0}")]
        [StringLength(500, MinimumLength = 10,ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự")]
        [Display(Name ="Nhập giới thiệu bản thân")]
        public string AboutMe { get; set; }
    }
}
