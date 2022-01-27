using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Controllers;
using WebApp.Interfaces;

namespace WebApp.Areas.Dashboard.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin, Moderator")]
    public class CommentController : BaseController
    {
        public CommentController(IRepositoryManager repository) : base(repository)
        {
        }

        public async Task<IActionResult> Index()
        {
            var comments = await _repository.Comment.GetComments();    
            return View(comments);
        }
        public async Task<IActionResult> Delete(int id)
        {
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            await _repository.Comment.DeleteComment(id, token);
            return RedirectToAction(nameof(Index));
        }
    }
}
