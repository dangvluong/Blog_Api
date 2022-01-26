using WebApp.Models;

namespace WebApp.ViewModels
{
    public class MemberDetailViewModel
    {
        public Member Member { get; set; }
        public int NumberOfPost { get; set; }
        public int NumberOfComment { get; set; }
    }
}
