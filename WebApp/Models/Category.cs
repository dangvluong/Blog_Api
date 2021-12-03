using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập tên danh mục")]
        [StringLength(64,MinimumLength =3,ErrorMessage = "Tên danh mục phải dài từ 3 đến 64 ký tự")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? ParentCategoryId { get; set; }
        public Category  ParentCategory { get; set; }
    }
}
