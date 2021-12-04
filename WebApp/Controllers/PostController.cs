using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PostController : BaseController
    {
        public PostController(SiteProvider siteHelper) : base(siteHelper)
        {
        }

        // GET: PostController
        public async Task<ActionResult> Index()
        {                      
            return View(await siteHelper.Post.GetPosts());
        }

        // GET: PostController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            Post post = await siteHelper.Post.GetPostById(id);
            if (post is null)
                return NotFound();
            return View(post);
        }

        // GET: PostController/Create
        public async Task<ActionResult> Create()
        {            
            ViewBag.categories = new SelectList(await siteHelper.Category.GetCategories(), "Id", "Name");
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Post post)
        {
            if (!ModelState.IsValid)
                return View(post);
            var result = await siteHelper.Post.Create(post);
            return RedirectToAction(nameof(Index));
        }

        // GET: PostController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Post post = await siteHelper.Post.GetPostById(id);
            if (post is null)
                return NotFound();
            ViewBag.categories = new SelectList(await siteHelper.Category.GetCategories(), "Id", "Name");
            return View(post);
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Post post)
        {
            if (post.Id != id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(post);
            await siteHelper.Post.Edit(post);
            return RedirectToAction(nameof(Index));
        }

        // GET: PostController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            Post post = await siteHelper.Post.GetPostById(id);
            if (post is null)
                return NotFound();
            return View(post);           
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmAsync(int id)
        {
            await siteHelper.Post.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
