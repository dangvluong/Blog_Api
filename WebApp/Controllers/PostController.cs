using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApp.DataTransferObject;
using WebApp.Helper;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

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
            if (post == null)
                return NotFound();
            post.Comments = await _repository.Comment.GetCommentsByPostId(post.Id);
            return View(post);
        }

        // GET: PostController/Create
        [Authorize]
        public async Task<ActionResult> Create()
        {
            List<Category> selectListCategories = await CreateSelectListCategory();
            ViewBag.categories = new SelectList(selectListCategories, "Id", "Name");
            return View();
        }

        // POST: PostController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post, IFormFile thumbnailImage)
        {
            if (!ModelState.IsValid)
            {
                List<Category> selectListCategory = await CreateSelectListCategory();
                ViewBag.categories = new SelectList(selectListCategory, "Id", "Name");
                return View(post);
            }
            post.AuthorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            post.DateCreated = DateTime.Now;
            if (thumbnailImage != null && !string.IsNullOrEmpty(thumbnailImage.FileName))
            {
                post.Thumbnail = await this.UploadThumbnail(thumbnailImage);
            }
            ResponseModel response = await _repository.Post.Create(post, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOption
                {
                    Type = "success",
                    Message = "Bài viết của bạn đã được tạo và chờ duyệt."
                });
                return RedirectToAction(nameof(Index));
            }
            return HandleErrors(response);
        }

        // GET: PostController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            Post post = await _repository.Post.GetPostById(id);
            if (post == null)
                return NotFound();
            //only author of post or admin can edit post            
            if (post.Author.Id == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) || User.IsInRole("Admin,Moderator"))
            {
                List<Category> selectListCategory = await CreateSelectListCategory();
                ViewBag.categories = new SelectList(selectListCategory, "Id", "Name");
                return View(post);
            }
            return BadRequest();
        }       

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Post post, IFormFile thumbnailImage)
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
            ResponseModel response = await _repository.Post.Edit(post, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOption
                {
                    Type = "success",
                    Message = "Đã cập nhật bài viết thành công"
                });
                return RedirectToAction(nameof(Detail), new { id = id });
            }
            return HandleErrors(response);
        }

        private async Task<string> UploadThumbnail(IFormFile thumbnailImage)
        {
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StreamContent(thumbnailImage.OpenReadStream()), nameof(thumbnailImage), thumbnailImage.FileName);
            string url = "/api/fileupload/postthumbnail";
            var response = await _repository.FileUpload.Upload(content, url, AccessToken);
            if (response is SuccessResponseModel)
            {
                return (string)response.Data;
            }
            return null;
        }

        // GET: PostController/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            Post post = await _repository.Post.GetPostById(id);
            if (post is null)
                return NotFound();
            //only author of post or admin can delete post            
            if (post.Author.Id == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) || User.IsInRole("Admin,Moderator"))
            {
                return View(post);
            }
            return BadRequest();

        }

        // POST: PostController/Delete/5
        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            ResponseModel response = await _repository.Post.Delete(id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOption
                {
                    Type = "success",
                    Message = "Đã cập nhật trạng thái bài viết thành công"
                });
                RedirectToAction(nameof(Index));
            }
            return HandleErrors(response);
        }
        public async Task<IActionResult> Search(string keyword, int page = 1)
        {
            if (string.IsNullOrEmpty(keyword))
                return BadRequest();
            ListPostDto searchResult = await _repository.Post.SearchPost(keyword, page);
            return View(searchResult);
        }

        private void CreateSelectItems(List<Category> source, List<Category> des, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("----;", level));
            foreach (var category in source)
            {
                category.Name = prefix + category.Name;
                des.Add(category);
                if (category.ChildCategories?.Count > 0)
                {
                    CreateSelectItems(category.ChildCategories, des, level + 1);
                }
            }
        }
    }
}
