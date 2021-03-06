using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.DataTransferObject;

namespace WebApi.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateModifier { get; set; }

        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Member Author { get; set; }
        public List<Comment> Comments { get; set; }
        public int CountView { get; set; }
        public string Thumbnail { get; set; }
    }
}
