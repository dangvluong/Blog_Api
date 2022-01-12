using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;

namespace WebApp.Controllers
{
    public class SeedDataController : BaseController
    {
        public SeedDataController(IRepositoryManager repository) : base(repository)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ClearData()
        {
            await _repository.SeedData.ClearAll();
            TempData["message"] = "Đã xoá data thành công";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SeedData()
        {
            await _repository.SeedData.SeedData();
            TempData["message"] = "Đã seed data thành công";
            return RedirectToAction(nameof(Index));
        }
    }
}
