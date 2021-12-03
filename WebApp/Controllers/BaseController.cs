using Microsoft.AspNetCore.Mvc;
using WebApp.Helper;

namespace WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly SiteHelper siteHelper;
        public BaseController(SiteHelper siteHelper)
        {
            this.siteHelper = siteHelper;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
