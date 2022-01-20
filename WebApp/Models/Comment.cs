using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int PostId { get; set; }        
        public DateTime DateCreate { get; set; }
    }
}
