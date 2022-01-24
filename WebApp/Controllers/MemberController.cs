﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class MemberController : BaseController
    {
        public MemberController(IRepositoryManager repository) : base(repository)
        {

        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            Member member = await _repository.Member.GetMemberById(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), token);
            return View(member);
        }
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel obj)
        {
            if (!ModelState.IsValid)
                return View();
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            //bool isOldPasswordValid = await _repository.Auth.CheckOldPasswordValid(changePasswordModel.OldPassword, token);
            //if (!isOldPasswordValid)
            //{
            //    ModelState.AddModelError(string.Empty, "Mật khẩu cũ không đúng");
            //    return View();
            //}                
            BadRequestResponse response = await _repository.Auth.ChangePassword(obj, token);
            if (response == null)
                return RedirectToAction("Logout", "Account");
            foreach (var errorMessage in response.Errors)
            {
                foreach (var error in errorMessage.Value)
                {
                    ModelState.AddModelError(errorMessage.Key, error);
                }
            }
            return View();
        }
        [Authorize]
        public async Task<IActionResult> ListPost(int? id = null)
        {
            if (id == null)
                id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ListPostByMemberViewModel viewModel = new ListPostByMemberViewModel
            {
                Member = await _repository.Member.GetMemberById(id.Value),
                Posts = await _repository.Post.GetPostsByMember(id.Value)
            };
            return View(viewModel);
        }
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> ChangeAboutMe()
        {
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            Member member = await _repository.Member.GetMemberById(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), token);
            return View(new ChangeAboutMeModel
            {
                MemberId = member.Id,
                AboutMe = member.AboutMe
            });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeAboutMe(ChangeAboutMeModel obj)
        {
            if (!ModelState.IsValid)
                return View();
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            int result = await _repository.Member.ChangeAboutMe(obj, token);
            return RedirectToAction(nameof(Index));

        }
        [Authorize]
        public IActionResult ChangeAvatar()
        {
            int memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(new ChangeAvatarModel
            {
                MemberId = memberId,
            });
        }
        [Authorize, HttpPost]
        public async Task<IActionResult> ChangeAvatar(ChangeAvatarModel obj)
        {
            //Implement validation for IFormFile: size, extension (?)
            //if (!ModelState.IsValid)
            //    return View();
            MultipartFormDataContent content = new MultipartFormDataContent();
            if (obj.AvatarUpload != null)
            {
                content.Add(new StringContent(obj.MemberId.ToString()), nameof(obj.MemberId));
                content.Add(new StreamContent(obj.AvatarUpload.OpenReadStream()), nameof(obj.AvatarUpload), obj.AvatarUpload.FileName);
            }
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            int result = await _repository.Member.ChangeAvatar(content, token);

            return RedirectToAction(nameof(Index));
        }
    }
}
