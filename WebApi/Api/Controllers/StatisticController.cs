using Microsoft.AspNetCore.Mvc;
using WebApi.DataTransferObject;
using WebApi.Interfaces;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class StatisticController : BaseController
    {
        public StatisticController(IRepositoryManager repository) : base(repository)
        { }

        [HttpGet]
        public async Task<ActionResult<StatisticDto>> GetStatistics()
        {
            StatisticDto dto = new StatisticDto()
            {
                TotalPost = await _repository.Post.CountTotalPost(),
                TotalUnapprovedPost = await _repository.Post.CountUnapprovedPost(),
                NewPosts = await _repository.Post.CountNewPost(),
                TotalMember = (await _repository.Member.GetMembers(trackChanges: false)).Count,
                NewMember = (await _repository.Member.GetNewMembers(trackChanges: false)).Count,
                TotalComment = (await _repository.Comment.GetComments()).Count
            };
            return Ok(dto);
        }
    }
}