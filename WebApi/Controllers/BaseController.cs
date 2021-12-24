using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly RepositoryManager _repository;        

        public BaseController(RepositoryManager repository)
        {
            _repository = repository;          
        }

    }
}
