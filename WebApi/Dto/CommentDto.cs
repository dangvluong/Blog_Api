namespace WebApi.Dto
{
    public class CommentDto
    {
        public int Id { get; set; }       
        public string Content { get; set; }
        public int AuthorId { get; set; }     
        public string AuthorName { get; set; }
        public int PostId { get; set; }
        public int? CommentParentId { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
