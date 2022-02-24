using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Controllers;
using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Models;
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
            //string token = User.FindFirst(Data.ClaimTypes.AccessToken).Value;
            List<Member> members = await _repository.Member.GetMembers(AccessToken);
            return View(members);
        }
        public async Task<IActionResult> BanAccount(int id, string returnUrl = "")
        {
            //string token = User.FindFirst(Data.ClaimTypes.AccessToken).Value;
            int result = await _repository.Member.BanAccount(id, AccessToken);
            if(!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int id)
        {
            //string token = User.FindFirst(Data.ClaimTypes.AccessToken).Value;
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
            //string token = User.FindFirst(Data.ClaimTypes.AccessToken).Value;
            Member member = await _repository.Member.GetMemberById(id);
            if (member == null)
                return NotFound();
            ViewBag.roles = await _repository.Role.GetRoles(AccessToken);
            return View(member);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRolesOfMemberDto obj)
        {
            //string token = User.FindFirst(Data.ClaimTypes.AccessToken).Value;
            int result = await _repository.Member.UpdateRolesOfMember(obj, AccessToken);
            return RedirectToAction(nameof(Index));
        }
    }
}
