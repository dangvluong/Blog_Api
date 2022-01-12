using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Controllers;
using WebApp.Interfaces;

namespace WebApp.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class CommentController : BaseController
    {
        public CommentController(IRepositoryManager repository) : base(repository)
        {
        }

        public async Task<IActionResult> Index()
        {
            var comments = (await _repository.Comment.GetComments()).OrderByDescending(p => p.DateCreate);           
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
