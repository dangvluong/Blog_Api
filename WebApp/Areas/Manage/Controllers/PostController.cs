using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers;
using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Moderator")]
    public class PostController : BaseController
    {
        public PostController(IRepositoryManager repository) : base(repository)
        {
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            ListPostDto listPostDto = await _repository.Post.ManagerGetPosts(page, AccessToken);
            return View(listPostDto);
        }

        public async Task<IActionResult> NewPost(int page = 1)
        {
            ListPostDto listPostDto = await _repository.Post.GetNewPosts(page, AccessToken);
            return View(listPostDto);
        }

        public async Task<IActionResult> ListUnapproved(int page = 1)
        {
            ListPostDto listPostDto = await _repository.Post.GetUnapprovedPosts(page, AccessToken);
            return View(listPostDto);
        }
        public async Task<IActionResult> Approve(int id, string returnUrl = "")
        {
            ResponseModel response = await _repository.Post.Approve(id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã phê duyệt bài viết"
                });
            }
            else
                HandleErrors(response);
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveApproved(int id, string returnUrl = "")
        {
            ResponseModel response = await _repository.Post.RemoveApproved(id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "warning",
                    Message = "Đã hủy bỏ phê duyệt bài viết"
                });
            }
            else
                HandleErrors(response);
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            ResponseModel response = await _repository.Post.Delete(id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "warning",
                    Message = "Đã xóa bài viết"
                });
            }
            else
                HandleErrors(response);
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Restore(int id)
        {
            ResponseModel response = await _repository.Post.Restore(id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "warning",
                    Message = "Đã khôi phục bài viết"
                });
            }
            else
                HandleErrors(response);
            return RedirectToAction(nameof(Index));
        }
    }
}
