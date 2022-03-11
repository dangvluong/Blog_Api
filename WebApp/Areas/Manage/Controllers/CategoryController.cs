using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApp.Controllers;
using WebApp.Helper;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Areas.Dashboard.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : BaseController
    {
        public CategoryController(IRepositoryManager repository) : base(repository)
        {
        }

        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _repository.Category.ManagerGetCategories(AccessToken);
            List<Category> listCategory = CategoryHelper.CreateTreeLevelCategory(categories);
            return View(listCategory);
        }
        public async Task<IActionResult> Create()
        {
            var selectListCategory = await CreateSelectListCategory();
            ViewBag.categories = new SelectList(selectListCategory, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, IFormFile thumbnailImage)
        {
            if (!ModelState.IsValid)
            {
                var selectListCategory = await CreateSelectListCategory();
                ViewBag.categories = new SelectList(selectListCategory, "Id", "Name");
                return View();
            }
            if (thumbnailImage != null && !string.IsNullOrEmpty(thumbnailImage.FileName))
            {
                category.Thumbnail = await UploadThumbnail(thumbnailImage);
            }
            var result = await _repository.Category.Create(category, AccessToken);
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Edit(int id)
        {
            List<Category> categories = await _repository.Category.GetCategories();
            Category targetCategory = categories.Where(c => c.Id == id).FirstOrDefault();
            if (targetCategory == null)
                return NotFound();
            //A category cannot select itself or it's child as parent
            categories.Remove(targetCategory);
            categories = CategoryHelper.CreateTreeLevelCategory(categories);
            var selectListCategory = new List<Category>();
            CategoryHelper.CreateSelectListCategory(categories, selectListCategory, 0);
            ViewBag.categories = new SelectList(selectListCategory, "Id", "Name");
            return View(targetCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category, IFormFile thumbnailImage)
        {
            if (category.Id != id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View();
            if (thumbnailImage != null && !string.IsNullOrEmpty(thumbnailImage.FileName))
            {
                category.Thumbnail = await UploadThumbnail(thumbnailImage);
            }
            ResponseModel response = await _repository.Category.Edit(category, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "success",
                    Message = "Đã cập nhật danh mục thành công."
                });
                return RedirectToAction(nameof(Index));
            }
            HandleErrors(response);
            return View();

        }
        public async Task<IActionResult> Delete(int id)
        {
            ResponseModel response = await _repository.Category.Delete(id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "warning",
                    Message = "Đã xóa danh mục."
                });
            }
            else
                HandleErrors(response);
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Restore(int id)
        {
            ResponseModel response = await _repository.Category.Delete(id, AccessToken);
            if (response is SuccessResponseModel)
            {
                PushNotification(new NotificationOptions
                {
                    Type = "info",
                    Message = "Đã khôi phục danh mục."
                });
            }
            else
                HandleErrors(response);
            return RedirectToAction(nameof(Index));

        }
    }
}
