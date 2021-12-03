using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Tên bài viết")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Content { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateModifier { get; set; }

        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int CategoryId { get; set; }        
        public Category Category { get; set; }
        public int? AuthorId { get; set; }
        
        //public User Author { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
