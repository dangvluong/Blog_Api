using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers;
using WebApp.DataTransferObject;
using WebApp.Interfaces;

namespace WebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    public class PostController : BaseController
    {
        public PostController(IRepositoryManager repository) : base(repository)
        {
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            ListPostDto listPostDto = await _repository.Post.GetPosts(page);
            return View(listPostDto);
        }
        public async Task<IActionResult> Approve(int id)
        {            
            int result = await _repository.Post.Approve(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
