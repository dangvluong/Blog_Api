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
    public class UserController : BaseController
    {
        public UserController(AppDbContext context) : base(context)
        {
        }
        [HttpPost("register")]
        public async Task<int> Register(RegisterModel model)
        {           
            context.Users.Add(new User
            {
                Username = model.Username,
                Password = SiteHelper.HashPassword(model.Password),
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                FullName = model.FullName,
                DateCreate = DateTime.Now,
            });           
            return await context.SaveChangesAsync();
        }
        [HttpGet]
        public async Task<List<User>> GetUsers()
        {
            return await context.Users.ToListAsync();
        }
        [HttpPost("login")]
        public async Task<User> Login(LoginModel model)
        {
            return await context.Users.Where(user =>
                user.Username == model.Username &&
                user.Password == SiteHelper.HashPassword(model.Password)
             ).Select(p => new User
             {
                 Id = p.Id,
                 Username = p.Username,
                 FullName = p.FullName,
                 Gender = p.Gender,
                 Email = p.Email,
                 DateOfBirth = p.DateOfBirth,
                 DateCreate = p.DateCreate

             }).FirstOrDefaultAsync();

        }
    }
}
