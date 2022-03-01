using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Moderator")]
    public class StatictisController : BaseController
    {
        public StatictisController(IRepositoryManager repository) : base(repository)
        {
        }
    }
}
