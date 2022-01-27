using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Controllers;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Manage")]
    public class RoleController : BaseController
    {
        public RoleController(IRepositoryManager repository) : base(repository)
        {
        }

        public async Task<IActionResult> Index()
        {
            string token = User.FindFirst(ClaimTypes.Authentication).Value;
            var roles =await _repository.Role.GetRoles(token);
            return View(roles);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Role obj)
        {
            if (!ModelState.IsValid)
                return View();
            string token = User.FindFirst(ClaimTypes.Authentication).Value;
            int result = await _repository.Role.CreateRole(obj, token);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            string token = User.FindFirst(ClaimTypes.Authentication).Value;
            Role role = await _repository.Role.GetRoleById(id, token);
            if (role == null)
                return NotFound();
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Role role)
        {
            if (!ModelState.IsValid)
                return View();
            string token = User.FindFirst(ClaimTypes.Authentication).Value;
            int result = await _repository.Role.UpdateRole(role, token);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            string token = User.FindFirst(ClaimTypes.Authentication).Value;
            int result = await _repository.Role.DeleteRole(id, token);
            return RedirectToAction(nameof(Index));

        }
    }
}
