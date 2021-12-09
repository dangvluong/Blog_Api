using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            context.Users.Add(new Member
            {
                Username = model.Username,
                Password = SiteHelper.HashPassword(model.Password),
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                FullName = model.FullName,
                DateCreate = DateTime.Now,
            });
            await context.SaveChangesAsync();
            return Ok();
                
        }
        [HttpGet]
        public async Task<List<Member>> GetUsers()
        {
            return await context.Users.ToListAsync();
        }
        [HttpPost("login")]
        public async Task<object> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            Member member =  await context.Users.Where(user =>
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
                 DateCreate = p.DateCreate

             }).FirstOrDefaultAsync();
            if(member != null)
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
                    Token = token
                };
            }
            return null;
        }
    }
}
