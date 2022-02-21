using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Xin lỗi, trang bạn tìm không có";
                    break;  
            }
            return View("NotFound");
                
        }   
        [Route("Error")]
        public IActionResult Index()
        {
            return View("Error");
        }
    }
}
