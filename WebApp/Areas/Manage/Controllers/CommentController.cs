using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Controllers;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Areas.Dashboard.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin, Moderator")]
    public class CommentController : BaseController
    {
        public CommentController(IRepositoryManager repository) : base(repository)
        {
        }

        public async Task<IActionResult> Index()
        {
            var comments = await _repository.Comment.GetComments(AccessToken);
            return View(comments);
        }
        public async Task<IActionResult> Delete(int id)
        {
            ResponseModel response = await _repository.Comment.DeleteComment(id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOption
                {
                    Type = "success",
                    Message = "Đã xóa bình luận"
                });
                return RedirectToAction(nameof(Index));
            }
            return HandleErrors(response);
        }
    }
}
