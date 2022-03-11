using Microsoft.AspNetCore.Mvc;
using WebApp.DataTransferObject;
using WebApp.Interfaces;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(IRepositoryManager repository) : base(repository)
        {
        }

        public async Task<IActionResult> Index(int id)
        {
            List<Category> categories = await _repository.Category.GetCategories();
            return View(categories);
        }
        public async Task<IActionResult> Detail(int id, int page = 1)
        {
            Category category = await _repository.Category.GetCategoryById(id);
            if (category == null)
                return BadRequest();
            ListPostDto listPostFromCategory =   await _repository.Post.GetPostsFromCategory(id, page);

            ListPostFromCategoryDto viewModel = new ListPostFromCategoryDto
            {
                Category = category,
                Posts = listPostFromCategory.Posts,
                TotalPage = listPostFromCategory.TotalPage
            };
            return View(viewModel);
        }
    }
}
