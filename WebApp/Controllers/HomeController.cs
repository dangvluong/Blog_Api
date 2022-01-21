using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IRepositoryManager repository) : base(repository)
        {
        }

        public async Task<IActionResult> Index()
        {
            HomeIndexViewModel viewModel = new HomeIndexViewModel
            {
                MostRecentPosts = await _repository.Post.GetMostRecentPosts(),
                TodayHighlightPosts = await _repository.Post.GetTodayHighlightPosts(),
                FeaturedPosts = await _repository.Post.GetFeaturedPosts()
            };
            return View(viewModel);
        }
    }
}
