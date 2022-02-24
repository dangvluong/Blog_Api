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
            var roles = await _repository.Role.GetRoles(AccessToken);
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
            int result = await _repository.Role.CreateRole(obj, AccessToken);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            Role role = await _repository.Role.GetRoleById(id, AccessToken);
            if (role == null)
                return NotFound();
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Role role)
        {
            if (!ModelState.IsValid)
                return View();
            int result = await _repository.Role.UpdateRole(role, AccessToken);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            int result = await _repository.Role.DeleteRole(id, AccessToken);
            return RedirectToAction(nameof(Index));

        }
    }
}
