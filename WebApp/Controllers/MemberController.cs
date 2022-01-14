using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Interfaces;
using WebApp.Models;

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
            Member member = await _repository.Member.GetMemberById(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),token);
            return View(member);
        }
    }
}
