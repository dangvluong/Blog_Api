using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;

namespace WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IRepositoryManager _repository;
        public BaseController(IRepositoryManager repository)
        {
            _repository = repository;
        }       
    }
}
