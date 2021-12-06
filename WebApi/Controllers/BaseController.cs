using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly AppDbContext context;        

        public BaseController(AppDbContext context)
        {
            this.context = context;          
        }

    }
}
