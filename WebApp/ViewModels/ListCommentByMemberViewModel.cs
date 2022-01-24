using WebApp.Models;

namespace WebApp.ViewModels
{
    public class ListCommentByMemberViewModel
    {
        public Member Member { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
