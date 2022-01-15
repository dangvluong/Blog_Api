using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PostController : BaseController
    {
        public PostController(IRepositoryManager repository) : base(repository)
        {
        }

        // GET: PostController
        public async Task<ActionResult> Index()
        {                      
            return View(await _repository.Post.GetPosts());
        }

        // GET: PostController/Detail/5
        public async Task<ActionResult> Detail(int id)
        {
            Post post = await _repository.Post.GetPostById(id);
            if (post is null)
                return NotFound();
            post.Comments = await _repository.Comment.GetCommentsByPostId(post.Id);
            return View(post);
        }

        // GET: PostController/Create
        [Authorize]
        public async Task<ActionResult> Create()
        {            
            ViewBag.categories = new SelectList(await _repository.Category.GetCategories(), "Id", "Name");
            return View();
        }

        // POST: PostController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Post post)
        {
            if (!ModelState.IsValid)
                return View(post);
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            post.AuthorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            post.DateCreated = DateTime.Now;
            var result = await _repository.Post.Create(post, token);
            return RedirectToAction(nameof(Index));
        }

        // GET: PostController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {   
            Post post = await _repository.Post.GetPostById(id);
            if (post is null)
                return NotFound();
            //only author of post or admin can edit post            
            if (post.AuthorId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return BadRequest();
            ViewBag.categories = new SelectList(await _repository.Category.GetCategories(), "Id", "Name");
            return View(post);
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(int id, Post post)
        {
            if (post.Id != id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(post);
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            post.DateModifier = DateTime.Now;
            await _repository.Post.Edit(post, token);
            return RedirectToAction(nameof(Index));
        }

        // GET: PostController/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {            
            Post post = await _repository.Post.GetPostById(id);
            if (post is null)
                return NotFound();
            //only author of post or admin can delete post            
            if (post.AuthorId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return BadRequest();
            return View(post);           
        }

        // POST: PostController/Delete/5
        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            await _repository.Post.Delete(id, token);
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public async Task<IActionResult> ListPostByMember(int? id = null)
        {
            if (id == null)
                id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var listPost = await _repository.Post.GetPostsByMember(id.Value);
            return View(listPost);
        }
    }
}
