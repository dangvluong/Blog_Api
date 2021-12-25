using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Controllers;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class CommentController : BaseController
    {
        public CommentController(RepositoryManager siteHelper) : base(siteHelper)
        {
        }

        public async Task<IActionResult> Index()
        {
            var comments = (await siteHelper.Comment.GetComments()).OrderByDescending(p => p.DateCreate);           
            return View(comments);
        }
        public async Task<IActionResult> Delete(int id)
        {
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            await siteHelper.Comment.DeleteComment(id, token);
            return RedirectToAction(nameof(Index));
        }
    }
}
