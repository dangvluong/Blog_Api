using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    public class MemberController : BaseController
    {
        public MemberController(IRepositoryManager repository) : base(repository)
        {

        }

        public async Task<IActionResult> Index()
        {
            Member member = await _repository.Member.GetMemberById(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), AccessToken);
            return View(member);
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel obj)
        {
            if (!ModelState.IsValid)
                return View();

            ResponseModel response = await _repository.Auth.ChangePassword(obj, AccessToken);            
            if (response is SuccessResponseModel)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã cập nhật mật khẩu mới. Bạn cần đăng nhập lại."
                });
                return RedirectToAction("Login", "Account");
            }
            HandleErrors(response);
            return View();
        }

        public async Task<IActionResult> ListPost(int? id = null)
        {
            if (id == null)
                id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ListPostByMemberViewModel viewModel = new ListPostByMemberViewModel
            {
                Member = await _repository.Member.GetMemberById(id.Value),
                Posts = await _repository.Post.GetPostsByMember(id.Value, AccessToken)
            };
            return View(viewModel);
        }

        public async Task<IActionResult> ListComment(int? id = null)
        {
            if (id == null)
                id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ListCommentByMemberViewModel viewModel = new ListCommentByMemberViewModel
            {
                Member = await _repository.Member.GetMemberById(id.Value),
                Comments = await _repository.Comment.GetCommentsByMember(id.Value)
            };
            return View(viewModel);
        }
        public async Task<IActionResult> ChangeAboutMe()
        {
            Member member = await _repository.Member.GetMemberById(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), AccessToken);
            return View(new ChangeAboutMeModel
            {
                MemberId = member.Id,
                AboutMe = member.AboutMe
            });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeAboutMe(ChangeAboutMeModel obj)
        {
            if (!ModelState.IsValid)
                return View();
            ResponseModel response = await _repository.Member.ChangeAboutMe(obj, AccessToken);            
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã cập nhật giới thiệu bản thân."
                });
                return RedirectToAction(nameof(Index));
            }
            HandleErrors(response);
            return View();
        }
        public IActionResult ChangeAvatar()
        {
            int memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(new ChangeAvatarModel
            {
                MemberId = memberId,
            });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeAvatar(ChangeAvatarModel obj)
        {
            //Implement validation for IFormFile: size, extension (?)
            if (!ModelState.IsValid)
                return View();
            MultipartFormDataContent content = new MultipartFormDataContent();
            if (obj.AvatarUpload != null)
            {
                content.Add(new StringContent(obj.MemberId.ToString()), nameof(obj.MemberId));
                content.Add(new StreamContent(obj.AvatarUpload.OpenReadStream()), nameof(obj.AvatarUpload), obj.AvatarUpload.FileName);
            }
            ResponseModel response = await _repository.Member.ChangeAvatar(content, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã cập nhật hình đại diện."
                });
                return RedirectToAction(nameof(Index));
            }
            HandleErrors(response);
            return View();
        }
    }
}
