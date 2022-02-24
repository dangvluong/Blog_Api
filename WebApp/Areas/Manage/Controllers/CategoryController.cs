using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApp.Controllers;
using WebApp.Helper;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Areas.Dashboard.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin")]
    public class CategoryController : BaseController
    {
        public CategoryController(IRepositoryManager repository) : base(repository)
        {
        }

        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            List<Category> categories = await _repository.Category.GetCategories();
            List<Category> listCategory = CategoryHelper.CreateTreeLevelCategory(categories);
            return View(listCategory);
        }

        // GET: CategoryController/Create
        public async Task<ActionResult> Create()
        {
            List<Category> sourceCategories = await _repository.Category.GetCategories();
            sourceCategories = CategoryHelper.CreateTreeLevelCategory(sourceCategories);
            List<Category> selectListItems = new List<Category>();
            CategoryHelper.CreateSelectListItem(sourceCategories, selectListItems, 0);
            ViewBag.categories = new SelectList(selectListItems, "Id", "Name");
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View();            
            var result = await _repository.Category.Create(category, AccessToken);
            return RedirectToAction(nameof(Index));

        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            List<Category> categories = await _repository.Category.GetCategories();
            Category targetCategory = categories.Where(c => c.Id == id).FirstOrDefault();
            if (targetCategory == null)
                return NotFound();
            //A category cannot select itself or it's child as parent
            categories.Remove(targetCategory);
            categories = CategoryHelper.CreateTreeLevelCategory(categories);
            var selectListItems = new List<Category>();
            CategoryHelper.CreateSelectListItem(categories, selectListItems, 0);

            ViewBag.categories = new SelectList(selectListItems, "Id", "Name");
            return View(targetCategory);
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
            await _repository.Category.Edit(category, AccessToken);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {            
            await _repository.Category.Delete(id, AccessToken);
            return RedirectToAction(nameof(Index));
        }
    }
}
