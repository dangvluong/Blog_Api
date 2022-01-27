using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Controllers;
using WebApp.DataTransferObject;
using WebApp.Interfaces;

namespace WebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin, Moderator")]
    public class PostController : BaseController
    {
        public PostController(IRepositoryManager repository) : base(repository)
        {
        }

        public async Task<IActionResult> Index(int id = 1)
        {
            ListPostDto listPostDto = await _repository.Post.GetPosts(id);
            return View(listPostDto);
        }
        public async Task<IActionResult> Approve(int id)
        {
            string token = User.FindFirst(ClaimTypes.Authentication).Value;
            int result = await _repository.Post.Approve(id, token);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            string token = User.FindFirst(ClaimTypes.Authentication).Value;
            int result = await _repository.Post.Delete(id,token);            
            return RedirectToAction(nameof(Index));
        }
    }
}
