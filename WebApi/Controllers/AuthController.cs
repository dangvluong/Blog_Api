﻿using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;
using WebApi.Helper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _configuration;
        public AuthController(IRepositoryManager repository, IConfiguration configuration) : base(repository)
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
                DateCreate = DateTime.Now
            };
            member.Roles = new List<Role>();
            var role = await _repository.Role.GetRoleByName("member", trackChanges: true);
            member.Roles.Add(role);
            _repository.Member.AddMember(member);
            await _repository.SaveChanges();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<object> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            //Member member = await _repository.Member.Login(loginModel);
            Member member = await _repository.Member.GetMemberByCondition(member =>
                member.Username == loginModel.Username && member.Password == SiteHelper.HashPassword(loginModel.Password)
            , trackChanges: false);
            if (member != null)
            {
                string token = SiteHelper.CreateToken(member, _configuration.GetSection("secretkey").ToString());
                return new
                {
                    Id = member.Id,
                    Username = member.Username,
                    FullName = member.FullName,
                    Gender = member.Gender,
                    Email = member.Email,
                    DateOfBirth = member.DateOfBirth,
                    DateCreate = member.DateCreate,
                    Roles = member.Roles,
                    Token = token
                };
            }
            return null;
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
            return Ok();
        }
    }

}

