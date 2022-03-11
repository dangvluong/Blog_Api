namespace WebApi.DataTransferObject
{
    public class CommentDto
    {        
        public int Id { get; set; }       
        public string Content { get; set; }
        public int PostId { get; set; }
        public PostDto Post { get; set; }
        public int AuthorId { get; set; }
        public MemberDto Author { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
