using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

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
            var exceptionDetails =  HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionDetails != null)
            {
                _logger.LogError("Unhandle exception.");
                _logger.LogError(exceptionDetails.Error.StackTrace);
            }
            return View("Error");
        }
    }
}
