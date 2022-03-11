using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataTransferObject;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController
    {
        public CommentController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {
        }       
        [HttpGet]
        [Authorize(Roles ="Admin,Moderator")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            IEnumerable<Comment> comments = await _repository.Comment.GetComments();
            if(comments == null)
                return NotFound();
            return Ok(MapComments(comments));
        }      
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(int id)
        {
            Comment comment = await _repository.Comment.GetCommentById(id, trackChanges: false);
            if (comment == null)
                return NotFound();
            return Ok(_mapper.Map<CommentDto>(comment));
        }      
        [HttpGet]
        [Route("GetCommentsByPost/{id}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByPost(int id)
        {
            IEnumerable<Comment> comments = await _repository.Comment.GetCommentsByPost(id, trackChanges: false);
            if (comments == null)
                return NotFound();           
            return Ok(MapComments(comments));
        }     
        [HttpPut]
        [Authorize]        
        public async Task<IActionResult> UpdateComment(Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Comment.UpdateComment(comment);
            await _repository.SaveChanges();
            return NoContent();
        }     
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostComment(Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Comment.AddComment(comment);
            await _repository.SaveChanges();
            return Ok();
        }    
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _repository.Comment.GetCommentById(id, trackChanges: true);
            if (comment == null)
            {
                return NotFound("Không tìm thấy bình luận");
            }
            _repository.Comment.DeleteComment(comment);
            await _repository.SaveChanges();
            return NoContent();
        }
        [HttpGet("getcommentsbymember/{id}")]        
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByMember(int id)
        {
            var comments = await _repository.Comment.GetCommentsByMember(id, trackChanges: false);
            if (comments == null)
                return NotFound();            
            return Ok(MapComments(comments));
        }
        private List<CommentDto> MapComments(IEnumerable<Comment> comments)
        {
            List<CommentDto> commentsDtos = new List<CommentDto>();
            foreach (var comment in comments)
            {
                CommentDto commentDto = _mapper.Map<CommentDto>(comment);
                commentsDtos.Add(commentDto);
            }
            return commentsDtos;
        }
    }
}