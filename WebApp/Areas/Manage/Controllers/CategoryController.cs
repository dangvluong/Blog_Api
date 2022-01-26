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
            IEnumerable<Category> categories = await _repository.Category.GetCategories();
            Dictionary<int, Category> dictCategory = new Dictionary<int, Category>();
            foreach (Category category in categories)
            {
                dictCategory[category.Id] = category;
            }
            List<Category> listCategory = new List<Category>();

            foreach (Category category in categories)
            {
                if(category.ParentCategoryId == null)
                    listCategory.Add(category);
                else
                {
                    if (dictCategory[category.ParentCategoryId.Value].ChildCategories == null)
                        dictCategory[category.ParentCategoryId.Value].ChildCategories = new List<Category>();
                    dictCategory[category.ParentCategoryId.Value].ChildCategories.Add(category);
                }
            }
            return View(listCategory);
        }        

        // GET: CategoryController/Create
        public async Task<ActionResult> Create()
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
            List<Category> listCategory = await _repository.Category.GetCategories();
            Category category = listCategory.Where(C => C.Id == id).FirstOrDefault();
            if (category == null)
                return NotFound();
            listCategory.Remove(category);
            ViewBag.categories = new SelectList(listCategory, "Id", "Name");
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
            string token = User.FindFirstValue(ClaimTypes.Authentication);
            await _repository.Category.Delete(id, token);
            return RedirectToAction(nameof(Index));
        }
    }
}
