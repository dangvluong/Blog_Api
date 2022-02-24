using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Bạn phải nhập nội dung bình luận")]
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public Member Author{ get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
