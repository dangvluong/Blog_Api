using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : BaseController
    {
        private readonly IConfiguration _configuration;

        public MemberController(RepositoryManager repository, IConfiguration configuration) : base(repository)
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
            var role = await _repository.Role.GetRoleByName("Member");           
            member.Roles.Add(role);
            _repository.Member.Add(member);            
            await _repository.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public async Task<IEnumerable<Member>> GetUsers()
        {
            return await _repository.Member.GetMembers();            
        }
        [HttpPost("login")]
        public async Task<object> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            Member member = await _repository.Member.Login(loginModel);            
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
        [HttpGet("{id}")]
        [Authorize]
        public async Task<Member> GetMemberById(int id)
        {
            //Will implement only members can get data about them, or admins can view data of all members.

            //
            return await _repository.Member.GetMember(id);
        }
        [HttpPost("checkoldpasswordvalid")]
        [Authorize]
        public async Task<IActionResult> CheckOldPasswordValid([FromBody]string oldPassword)
        {
            if (string.IsNullOrEmpty(oldPassword) || oldPassword.Length < 6  || oldPassword.Length > 64)
                return BadRequest();
            var memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool isCurrentPasswordValid = await _repository.Member.CheckCurrentPasswordValid(memberId, oldPassword);            
            if (isCurrentPasswordValid)
                return Ok();
            return BadRequest();
        }
        [HttpPost("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword) && (newPassword.Length < 6 || newPassword.Length > 64))
                return BadRequest();
            int memberId =int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Member member =await _repository.Member.GetMember(memberId);
            if (member == null)
                return BadRequest();
            member.Password = SiteHelper.HashPassword(newPassword);
            await _repository.SaveChanges();
            return Ok();
        }         
    }
}
