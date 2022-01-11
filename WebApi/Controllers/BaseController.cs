using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
namespace WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IRepositoryManager _repository;        

        public BaseController(IRepositoryManager repository)
        {
            _repository = repository;          
        }

    }
}
