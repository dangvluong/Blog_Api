using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;

namespace WebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IRepositoryManager _repository;
        protected readonly IConfiguration _configuration;
        public BaseController(IRepositoryManager repository)
        {
            _repository = repository;
        }

        protected BaseController(IRepositoryManager repository, IConfiguration configuration) : this(repository)
        {
            _configuration = configuration;
        }
    }
}
