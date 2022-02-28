using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Controllers;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

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
            ResponseModel response = await _repository.Role.CreateRole(obj, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã tạo vai trò mới."
                });
                return RedirectToAction(nameof(Index));
            }
            HandleErrors(response);
            return View();

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
            ResponseModel response = await _repository.Role.UpdateRole(role, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã cập nhật vai trò."
                });
                return RedirectToAction(nameof(Index));
            }
            HandleErrors(response);
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            ResponseModel response = await _repository.Role.DeleteRole(id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã cập nhật trạng thái của vai trò."
                });
            }
            else
                HandleErrors(response);
            return RedirectToAction(nameof(Index));
        }
    }
}
