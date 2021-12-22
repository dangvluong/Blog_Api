using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : BaseController
    {
        private readonly IConfiguration configuration;
        public MemberController(AppDbContext context, IConfiguration configuration) : base(context)
        {
            this.configuration = configuration;
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
            var role = context.Roles.FirstOrDefault(s=> s.Name == "Member");
            member.Roles.Add(role);            
            context.Members.Add(member);
            await context.SaveChangesAsync();
            return Ok();

        }
        [HttpGet]
        public async Task<List<Member>> GetUsers()
        {
            return await context.Members.Include(p=>p.Roles).ToListAsync();
        }
        [HttpPost("login")]
        public async Task<object> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            Member member = await context.Members.Include(usr => usr.Roles).Where(user =>
               user.Username == model.Username &&
               user.Password == SiteHelper.HashPassword(model.Password)
             ).Select(p => new Member
             {
                 Id = p.Id,
                 Username = p.Username,
                 FullName = p.FullName,
                 Gender = p.Gender,
                 Email = p.Email,
                 DateOfBirth = p.DateOfBirth,
                 DateCreate = p.DateCreate,
                 Roles = p.Roles

             }).FirstOrDefaultAsync();
            if (member != null)
            {
                string token = SiteHelper.CreateToken(member, configuration.GetSection("secretkey").ToString());
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
            return await context.Members.FirstOrDefaultAsync(m => m.Id == id);            
        }
        
    }
}
