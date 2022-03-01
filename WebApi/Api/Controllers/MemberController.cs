using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DataTransferObject;
using WebApi.Enums;
using WebApi.Helper;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : BaseController
    {
        public MemberController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {

        }
        [HttpGet]
        //Should only admin view all members
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            IEnumerable<Member> members = await _repository.Member.GetMembers(trackChanges: false);
            if (members == null)
                return NotFound();
            return Ok(members);
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<MemberDto>> GetMemberById(int id)
        {
            //Will implement only members can get data about them, or admins can view data of all members.

            var member = await _repository.Member.GetMemberByCondition(member => member.Id == id, trackChanges: false);
            if (member == null)
                return NotFound();
            return _mapper.Map<MemberDto>(member);
        }
        [HttpPost("changeaboutme")]
        [Authorize]
        public async Task<IActionResult> ChangeAboutMe(ChangeAboutMeModel obj)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            int memberId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (memberId != obj.MemberId)
                return BadRequest();
            Member member = await _repository.Member.GetMemberByCondition(m => m.Id == obj.MemberId, trackChanges: true);
            if (member == null)
                return BadRequest();
            member.AboutMe = obj.AboutMe;
            await _repository.SaveChanges();
            return NoContent();
        }
        [HttpPost("changeavatar")]
        [Authorize]
        public async Task<IActionResult> ChangeAvatar([FromForm] ChangeAvatarModel obj)
        {
            int memberId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Member member = await _repository.Member.GetMemberByCondition(m => m.Id == memberId, trackChanges: true);
            if (member == null || member.Id != obj.MemberId || obj.AvatarUpload == null)
                return BadRequest();
            member.AvatarUrl = SiteHelper.UploadFile(obj.AvatarUpload, UploadTypes.Avatar);
            await _repository.SaveChanges();
            return NoContent();
        }
        [HttpPost("banaccount/{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> BanAccount(int id)
        {
            Member member = await _repository.Member.GetMemberByCondition(c => c.Id == id, trackChanges: true);
            if (member == null)
                return NotFound();
            if(member.IsBanned)
                return BadRequest("Tài khoản này đã bị khóa trước đó.");
            member.IsBanned = true;
            await _repository.SaveChanges();
            return NoContent();
        }
        [HttpPost("unbanaccount/{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> UnbanAccount(int id)
        {
            Member member = await _repository.Member.GetMemberByCondition(c => c.Id == id, trackChanges: true);
            if (member == null)
                return NotFound();
            if (!member.IsBanned)
                return BadRequest("Tài khoản này chưa bị khóa trước đó.");
            member.IsBanned = false;
            await _repository.SaveChanges();
            return NoContent();
        }

        [HttpPost("updaterolesofmember")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRolesOfMember(UpdateRolesOfMemberDto obj)
        {
            if (obj == null || obj.RoleIds.Length <= 0)
                return BadRequest();
            Member member = await _repository.Member.GetMemberByCondition(m => m.Id == obj.MemberId, trackChanges: true);
            if (member == null)
                return NotFound("Không tìm thấy thành viên");
            member.Roles.Clear();
            IEnumerable<Role> roles = await _repository.Role.GetRoles(trackChanges: true);
            foreach (Role role in roles)
            {
                if (obj.RoleIds.Contains(role.Id))
                    member.Roles.Add(role);
            }
            await _repository.SaveChanges();
            return NoContent();
        }
    }
}
