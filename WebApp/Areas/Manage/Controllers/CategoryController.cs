using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApp.Controllers;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Areas.Dashboard.Controllers
{
    [Area("Manage")]
    public class CategoryController : BaseController
    {
        public CategoryController(IRepositoryManager repository) : base(repository)
        {
        }

        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            return View(await _repository.Category.GetCategories());
        }

        // GET: CategoryController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await _repository.Category.GetCategoryById(id));
        }

        // GET: CategoryController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.categories = new SelectList(await _repository.Category.GetCategories(), "Id", "Name");
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
            var result = await _repository.Category.Create(category, token);
            return RedirectToAction(nameof(Index));
            
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            
            var category = await _repository.Category.GetCategoryById(id);
            if (category == null)
                return NotFound();
            ViewBag.categories = new SelectList(await _repository.Category.GetCategories(), "Id", "Name");
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
            await _repository.Category.Edit(category, token);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Category category = await _repository.Category.GetCategoryById(id);
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
            await _repository.Category.Delete(id, token);
            return RedirectToAction(nameof(Index));
        }
    }
}
