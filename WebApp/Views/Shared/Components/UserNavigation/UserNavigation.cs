using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Views.Shared.Components.UserNavigation
{
    [ViewComponent]
    [Authorize]
    public class UserNavigation : ViewComponent
    {
        private readonly IRepositoryManager _repository;
        public UserNavigation(IRepositoryManager repository)
        {
            _repository = repository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = User as ClaimsPrincipal;
            int memberId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            string token = user.FindFirstValue(Data.ClaimTypes.AccessToken);
            Member member = await _repository.Member.GetMemberById(memberId, token);
            return View(member);
        }
    }
}
