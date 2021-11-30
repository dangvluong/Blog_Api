using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        //public string ShortContent { get; set; }
        public string Content { get; set; }
        public bool isActive { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public User Author { get; set; }

    }
}
