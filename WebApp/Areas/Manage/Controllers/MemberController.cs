using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers;
using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;
using WebApp.ViewModels;

namespace WebApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin, Moderator")]
    public class MemberController : BaseController
    {
        public MemberController(IRepositoryManager repository) : base(repository)
        {
        }

        public async Task<IActionResult> Index()
        {
            List<Member> members = await _repository.Member.GetMembers(AccessToken);
            return View(members);
        }
        public async Task<IActionResult> BanAccount(int id, string returnUrl = "")
        {
            ResponseModel response = await _repository.Member.BanAccount(id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "warning",
                    Message = "Đã khóa tài khoản."
                });
            }
            else
                HandleErrors(response);
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> UnbanAccount(int id, string returnUrl = "")
        {
            ResponseModel response = await _repository.Member.UnbanAccount(id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "warning",
                    Message = "Đã mở khóa tài khoản."
                });
            }
            else
                HandleErrors(response);
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            Member member = await _repository.Member.GetMemberById(id);
            if (member == null)
                return NotFound();
            MemberDetailViewModel viewModel = new MemberDetailViewModel
            {
                Member = member,
                NumberOfComment = (await _repository.Comment.GetCommentsByMember(id)).Count,
                NumberOfPost = (await _repository.Post.GetPostsByMember(id)).Count
            };
            return View(viewModel);
        }
        public async Task<IActionResult> UpdateRole(int id)
        {
            Member member = await _repository.Member.GetMemberById(id);
            if (member == null)
                return NotFound();
            ViewBag.roles = await _repository.Role.GetRoles(AccessToken);
            return View(member);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRolesOfMemberDto obj)
        {
            ResponseModel response = await _repository.Member.UpdateRolesOfMember(obj, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã cập nhật vai trò của tài khoản."
                });
            }
            else
                HandleErrors(response);
            return RedirectToAction(nameof(Detail), new { id = obj.MemberId });
        }
    }
}
