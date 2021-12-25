using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApp.Controllers;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Areas.Dashboard.Controllers
{
    [Area("dashboard")]
    public class CategoryController : BaseController
    {
        public CategoryController(RepositoryManager siteHelper) : base(siteHelper)
        {
        }

        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            return View(await siteHelper.Category.GetCategories());
        }

        // GET: CategoryController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await siteHelper.Category.GetCategoryById(id));
        }

        // GET: CategoryController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.categories = new SelectList(await siteHelper.Category.GetCategories(), "Id", "Name");
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View();
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            var result = await siteHelper.Category.Create(category, token);
            return RedirectToAction(nameof(Index));
            
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            
            var category = await siteHelper.Category.GetCategoryById(id);
            if (category == null)
                return NotFound();
            ViewBag.categories = new SelectList(await siteHelper.Category.GetCategories(), "Id", "Name");
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Category category)
        {
            if (category.Id != id)
                return BadRequest();
            if (!ModelState.IsValid)
                return View();
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            await siteHelper.Category.Edit(category, token);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Category category = await siteHelper.Category.GetCategoryById(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            await siteHelper.Category.Delete(id, token);
            return RedirectToAction(nameof(Index));
        }
    }
}
