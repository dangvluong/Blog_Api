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
            List<Category> categories = await _repository.Category.GetCategories();
            List<Category> listCategory = CreateTreeLevelCategory(categories);
            return View(listCategory);
        }

        // GET: CategoryController/Create
        public async Task<ActionResult> Create()
        {
            List<Category> sourceCategories = await _repository.Category.GetCategories();
            sourceCategories = CreateTreeLevelCategory(sourceCategories);
            List<Category> selectListItems = new List<Category>();
            CreateSelectListItem(sourceCategories, selectListItems, 0);
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
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            var result = await _repository.Category.Create(category, token);
            return RedirectToAction(nameof(Index));

        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            List<Category> categories = await _repository.Category.GetCategories();
            Category targetCategory = categories.Where(c => c.Id == id).FirstOrDefault();
            if (targetCategory == null)
                return NotFound();
            categories.Remove(targetCategory);
            categories = CreateTreeLevelCategory(categories);
            var selectListItems = new List<Category>();
            CreateSelectListItem(categories, selectListItems, 0);

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
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            await _repository.Category.Edit(category, token);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            await _repository.Category.Delete(id, token);
            return RedirectToAction(nameof(Index));
        }



        private static List<Category> CreateTreeLevelCategory(List<Category> source)
        {
            Dictionary<int, Category> dictCategory = new Dictionary<int, Category>();
            foreach (Category category in source)
            {
                dictCategory[category.Id] = category;
            }
            List<Category> listCategory = new List<Category>();

            foreach (Category category in source)
            {
                if (category.ParentCategoryId == null)
                    listCategory.Add(category);
                else
                {
                    if (dictCategory.ContainsKey(category.ParentCategoryId.Value))
                    {
                        if (dictCategory[category.ParentCategoryId.Value].ChildCategories == null)
                            dictCategory[category.ParentCategoryId.Value].ChildCategories = new List<Category>();
                        dictCategory[category.ParentCategoryId.Value].ChildCategories.Add(category);
                    }
                }
            }

            return listCategory;
        }

        private void CreateSelectListItem(List<Category> source, List<Category> des, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("----", level));
            foreach (var category in source)
            {
                category.Name = prefix + category.Name;
                des.Add(category);
                if (category.ChildCategories?.Count > 0)
                {
                    CreateSelectListItem(category.ChildCategories, des, level + 1);
                }
            }
        }
    }
}
