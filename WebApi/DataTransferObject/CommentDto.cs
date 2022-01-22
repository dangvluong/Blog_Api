using WebApi.Models;

namespace WebApi.DataTransferObject
{
    public class CommentDto
    {        
        public int Id { get; set; }       
        public string Content { get; set; }
        public MemberDto Author { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
