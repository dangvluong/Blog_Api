﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DataTransferObject;
using WebApi.Helper;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _configuration;
        public AuthController(IRepositoryManager repository, IMapper mapper, IConfiguration configuration) : base(repository, mapper)
        {
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            Member member = new Member
            {
                Username = model.Username,
                Password = SiteHelper.HashPassword(model.Password),
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                FullName = model.FullName,
                DateCreate = DateTime.Now,
                AvatarUrl = DefaultValue.Avatar,
                AboutMe = DefaultValue.About
            };
            member.Roles = new List<Role>();
            var role = await _repository.Role.GetRoleByName("Member", trackChanges: true);
            member.Roles.Add(role);
            _repository.Member.AddMember(member);
            await _repository.SaveChanges();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<MemberDto>> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            //Member member = await _repository.Member.Login(loginModel);
            Member member = await _repository.Member.GetMemberByCondition(member =>
                member.Username == loginModel.Username && member.Password == SiteHelper.HashPassword(loginModel.Password)
            , trackChanges: false);
            if (member != null)
            {
                MemberDto memberDto = _mapper.Map<MemberDto>(member);
                memberDto.Token = SiteHelper.CreateJWToken(member, _configuration.GetSection("secretkey").ToString());
                return Ok(memberDto);
            }
            return BadRequest();
        }

        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel obj)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Member member = await _repository.Member.GetMemberByCondition(member =>
                   member.Id == memberId, trackChanges: true
               );
            if (member == null)
                return BadRequest();
            if (!member.Password.SequenceEqual(SiteHelper.HashPassword(obj.OldPassword)))
            {
                ModelState.AddModelError(nameof(obj.OldPassword), "Mật khẩu cũ không đúng");
                return ValidationProblem(ModelState);
            }
            member.Password = SiteHelper.HashPassword(obj.NewPassword);
            await _repository.SaveChanges();
            return NoContent();
        }
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromForm]string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();
            Member member = await _repository.Member.GetMemberByCondition(m => m.Email == email, trackChanges: true);
            if (member == null)
            {
                ModelState.AddModelError(nameof(member.Email), "Email không tồn tại");
                return ValidationProblem(ModelState);
            }
            if (member.IsBanned)
            {
                ModelState.AddModelError(nameof(member.Username), "Tài khoản của bạn đã bị khóa.");
                return ValidationProblem(ModelState);              
            }
            else
            {
                member.ResetPasswordToken = SiteHelper.CreateToken(32);
                await _repository.SaveChanges();
                ResetPasswordDto resetPasswordDto = new ResetPasswordDto
                {
                    Token = member.ResetPasswordToken,
                    Email = email,
                };
                return Ok(resetPasswordDto);
            }

        }
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto obj)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            Member member = await _repository.Member.GetMemberByCondition(m => m.Email == obj.Email && m.ResetPasswordToken == obj.Token, trackChanges: true);
            if (member == null)
                return BadRequest();
            member.Password = SiteHelper.HashPassword(obj.NewPassword);
            member.ResetPasswordToken = null;
            await _repository.SaveChanges();
            return NoContent();
        }
    }

}

