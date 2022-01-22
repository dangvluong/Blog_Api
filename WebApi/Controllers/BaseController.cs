using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
namespace WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IRepositoryManager _repository;
        protected readonly IMapper _mapper;

        public BaseController(IRepositoryManager repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public BaseController(IRepositoryManager repository)
        {
            _repository = repository;          
        }

    }
}
