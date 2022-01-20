using WebApi.Models;

namespace WebApi.DataTransferObject
{
    public class ListPostDto
    {
        public int TotalPage { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
