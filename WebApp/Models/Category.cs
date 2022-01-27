using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập tên danh mục")]
        [StringLength(64,MinimumLength =3,ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự")]
        [Display(Name ="Tên danh mục")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Display(Name = "Danh mục cha")]
        public int? ParentCategoryId { get; set; }
        public Category  ParentCategory { get; set; }
        public List<Category> ChildCategories { get; set; }
    }
}
