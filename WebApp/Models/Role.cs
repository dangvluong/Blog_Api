using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Bạn phải nhập {0}")]
        [StringLength(32, MinimumLength = 5, ErrorMessage ="{0} phải dài từ {2} đến {1} ký tự")]
        [Display(Name ="Tên vài trò")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name ="Màu hiển thị")]
        public bool  CanChange { get; set; } = true;
        public string ColorDisplay { get; set; }
    }
}
