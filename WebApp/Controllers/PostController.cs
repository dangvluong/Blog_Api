using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PostController : Controller
    {
        public async Task<IActionResult> Index()
        {
            PostRepository postRepository = new PostRepository();   

            return View(await postRepository.GetPosts());
        }
    }
}
