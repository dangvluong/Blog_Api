using WebApp.Models;

namespace WebApp.ViewModels
{
    public class ListPostByMemberViewModel
    {
        public Member Member{ get; set; }
        public List<Post> Posts { get; set; }
    }
}
