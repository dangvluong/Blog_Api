using WebApp.Models;

namespace WebApp.DataTransferObject
{
    public class ListPostDto
    {
        public int TotalPage { get; set; }        
        public List<Post> Posts { get; set; }
    }
}
