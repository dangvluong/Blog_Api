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
      
        public async Task<ActionResult> Index(int page = 1)
        {
            ListPostDto listPost = await _repository.Post.GetPosts(page);
            return View(listPost);
        }
       
        public async Task<ActionResult> Detail(int id)
        {
            Post post = await _repository.Post.GetPostById(id, countView: true);
            if (post == null)
                return NotFound();
            //Only author or admin/moderator can view inactive post detail
            if (post.IsActive == false && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
            {
                int userId;
                bool isUserIdExist = int.TryParse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier), out userId);
                if(userId != post.AuthorId)
                    return BadRequest();
            }
            post.Comments = await _repository.Comment.GetCommentsByPostId(post.Id);
            return View(post);
        }
        
        [Authorize]
        public async Task<ActionResult> Create()
        {
            List<Category> selectListCategories = await CreateSelectListCategory();
            ViewBag.categories = new SelectList(selectListCategories, "Id", "Name");
            return View();
        }
      
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
            post.AuthorId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier));
            post.DateCreated = DateTime.Now;
            if (thumbnailImage != null && !string.IsNullOrEmpty(thumbnailImage.FileName))
            {
                post.Thumbnail = await UploadThumbnail(thumbnailImage);
            }
            ResponseModel response = await _repository.Post.Create(post, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Bài viết của bạn đã được tạo và chờ duyệt."
                });
                return RedirectToAction(nameof(Index));
            }
            HandleErrors(response);
            return View(post);
        }
            
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            Post post = await _repository.Post.GetPostById(id);
            if (post == null)
                return NotFound();                
            if (post.Author.Id == int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)) || User.IsInRole("Admin") || User.IsInRole("Moderator"))
            {
                List<Category> selectListCategory = await CreateSelectListCategory();
                ViewBag.categories = new SelectList(selectListCategory, "Id", "Name");
                return View(post);
            }
            return BadRequest();
        }

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
                post.Thumbnail = await UploadThumbnail(thumbnailImage);
            }
            ResponseModel response = await _repository.Post.Edit(post, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã cập nhật bài viết thành công"
                });
                return RedirectToAction(nameof(Detail), new { id = id });
            }
            HandleErrors(response);
            return View(post);
        }
        
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            Post post = await _repository.Post.GetPostById(id);
            if (post is null)
                return NotFound();
            //only author of post or admin can delete post            
            if (post.Author.Id == int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)) || User.IsInRole("Admin,Moderator"))
            {
                return View(post);
            }
            return BadRequest();

        }
               
        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            ResponseModel response = await _repository.Post.Delete(id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã cập nhật trạng thái bài viết thành công"
                });
            }
            else
                HandleErrors(response);
            return RedirectToAction(nameof(Index));
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
