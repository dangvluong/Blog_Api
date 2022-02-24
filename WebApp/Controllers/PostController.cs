using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApp.DataTransferObject;
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
        public async Task<ActionResult> Index(int page = 1)
        {
            ListPostDto listPost = await _repository.Post.GetPosts(page);
            return View(listPost);
        }

        // GET: PostController/Detail/5
        public async Task<ActionResult> Detail(int id)
        {
            Post post = await _repository.Post.GetPostById(id, countView: true);
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
        public async Task<ActionResult> Create(Post post, IFormFile thumbnailImage)
        {
            if (!ModelState.IsValid)
                return View(post);
            post.AuthorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            post.DateCreated = DateTime.Now;
            if (thumbnailImage != null && !string.IsNullOrEmpty(thumbnailImage.FileName))
            {
                post.Thumbnail = await this.UploadThumbnail(thumbnailImage);
            }
            var result = await _repository.Post.Create(post, AccessToken);
            return RedirectToAction(nameof(Index));
        }

        // GET: PostController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            Post post = await _repository.Post.GetPostById(id);
            if (post == null)
                return NotFound();
            //only author of post or admin can edit post            
            if (post.Author.Id != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return BadRequest();
            ViewBag.categories = new SelectList(await _repository.Category.GetCategories(), "Id", "Name");
            return View(post);
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(int id, Post post, IFormFile thumbnailImage)
        {
            if (post.Id != id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(post);
            post.DateModifier = DateTime.Now;
            if (thumbnailImage != null && !string.IsNullOrEmpty(thumbnailImage.FileName))
            {
                post.Thumbnail = await this.UploadThumbnail(thumbnailImage);
            }
            await _repository.Post.Edit(post, AccessToken);
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> UploadThumbnail(IFormFile thumbnailImage)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StreamContent(thumbnailImage.OpenReadStream()), nameof(thumbnailImage), thumbnailImage.FileName);
            string url = "/api/fileupload/postthumbnail";
            return await _repository.FileUpload.Upload(content, url);
        }

        // GET: PostController/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            Post post = await _repository.Post.GetPostById(id);
            if (post is null)
                return NotFound();
            //only author of post or admin can delete post            
            if (post.Author.Id != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
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
            await _repository.Post.Delete(id, AccessToken);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Search(string keyword, int page = 1)
        {
            if (string.IsNullOrEmpty(keyword))
                return BadRequest();
            ListPostDto searchResult = await _repository.Post.SearchPost(keyword, page);
            return View(searchResult);
        }
    }
}
