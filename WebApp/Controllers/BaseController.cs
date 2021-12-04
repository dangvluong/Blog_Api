using Microsoft.AspNetCore.Mvc;
using WebApp.Helper;

namespace WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly SiteProvider siteHelper;
        public BaseController(SiteProvider siteHelper)
        {
            this.siteHelper = siteHelper;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
