using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class CommentController : BaseController
    {
        public CommentController(IRepositoryManager repository) : base(repository)
        {
        }
        [HttpPost]
        public async Task<IActionResult> PostComment(Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            comment.AuthorId = userId;
            comment.DateCreate = DateTime.UtcNow;
            ResponseModel response = await _repository.Comment.PostComment(comment, AccessToken);
            if (response is SuccessResponseModel)
            {
                return RedirectToAction("Detail", "Post", new { Id = comment.PostId }, "comments");
            }
            return BadRequest();
        }

        public async Task<IActionResult> Edit(int id)
        {
            Comment comment = await _repository.Comment.GetComment(id);
            if (comment == null)
                return NotFound();
            return View(comment);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            ResponseModel response = await _repository.Comment.UpdateComment(comment, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã cập nhật bình luận"
                });
                return RedirectToAction("Detail", "Post", new { Id = comment.PostId }, "comments");
            }
            return BadRequest();
        }
        public async Task<IActionResult> Delete(int id)
        {
            Comment comment = await _repository.Comment.GetComment(id);
            if (comment == null)
                return NotFound();
            ResponseModel response = await _repository.Comment.DeleteComment(comment.Id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type="success",
                    Message="Đã xóa bình luận"
                });
                return RedirectToAction("Detail", "Post", new { Id = comment.PostId }, "comments");

            }
            return BadRequest();
        }
    }
}
