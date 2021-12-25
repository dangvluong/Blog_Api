using Microsoft.AspNetCore.Mvc;
using WebApp.Helper;

namespace WebApp.Controllers
{
    public class SeedDataController : BaseController
    {
        public SeedDataController(RepositoryManager siteHelper) : base(siteHelper)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ClearData()
        {
            await siteHelper.SeedData.ClearAll();
            TempData["message"] = "Đã xoá data thành công";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> SeedData()
        {
            await siteHelper.SeedData.SeedData();
            TempData["message"] = "Đã seed data thành công";
            return RedirectToAction(nameof(Index));
        }
    }
}
