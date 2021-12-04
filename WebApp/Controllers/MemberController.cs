using Microsoft.AspNetCore.Mvc;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class MemberController : Controller
    {
        private readonly SiteProvider siteHelper;
        public MemberController(SiteProvider siteHelper)
        {
            this.siteHelper =  siteHelper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]  
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if(!ModelState.IsValid)
                return View();
            await siteHelper.Member.Register(model);
            return RedirectToAction(nameof(Login));
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View();
            Member member = await siteHelper.Member.Login(model);
            if (member == null)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không chính xác");
                return View();
            }
            return Redirect("/");
        }
    }
}
