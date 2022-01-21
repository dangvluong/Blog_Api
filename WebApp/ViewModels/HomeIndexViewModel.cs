using WebApp.Models;

namespace WebApp.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Post> MostRecentPosts { get; set; }
        public IEnumerable<Post> TodayHighlightPosts { get; set; }
        public List<Post> FeaturedPosts { get; set; }
    }
}
