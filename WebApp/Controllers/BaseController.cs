using Microsoft.AspNetCore.Mvc;
using WebApp.Helper;

namespace WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly RepositoryManager siteHelper;
        public BaseController(RepositoryManager siteHelper)
        {
            this.siteHelper = siteHelper;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
