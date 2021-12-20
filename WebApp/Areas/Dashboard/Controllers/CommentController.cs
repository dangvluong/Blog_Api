using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class CommentController : BaseController
    {
        public CommentController(SiteProvider siteHelper) : base(siteHelper)
        {
        }

        public async Task<IActionResult> Index()
        {
            var comments = (await siteHelper.Comment.GetComments()).OrderByDescending(p => p.DateCreate);           
            return View(comments);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await siteHelper.Comment.DeleteComment(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
