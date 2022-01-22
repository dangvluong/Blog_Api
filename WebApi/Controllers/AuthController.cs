using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;
using WebApi.Helper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebApi.DataTransferObject;
using AutoMapper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _configuration;
        public AuthController(IRepositoryManager repository,IMapper mapper, IConfiguration configuration) : base(repository,mapper)
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
                memberDto.Token = SiteHelper.CreateToken(member, _configuration.GetSection("secretkey").ToString());
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
    }

}

