using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataTransferObject;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController
    {
        public CommentController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {
        }

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            Comment comment = await _repository.Comment.GetCommentById(id, trackChanges: false);
            if (comment == null)
                return NotFound();
            return Ok(comment);
        }

        // GET: api/GetCommentsByPost/5
        [HttpGet]
        [Route("GetCommentsByPost/{id}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByPost(int id)
        {
            IEnumerable<Comment> comments = await _repository.Comment.GetCommentsByPost(id, trackChanges: false);
            if (comments == null)
                return NotFound();
            List<CommentDto> commentsDtos = new List<CommentDto>();
            foreach (var comment in comments)
            {
                CommentDto commentDto = _mapper.Map<CommentDto>(comment);
                commentsDtos.Add(commentDto);
            }
            return Ok(commentsDtos);
        }

        // PUT: api/Comment/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Comment.UpdateComment(comment);
            await _repository.SaveChanges();
            return NoContent();
        }

        // POST: api/Comment        
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

        // DELETE: api/Comment/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _repository.Comment.GetCommentById(id, trackChanges: true);
            if (comment == null)
            {
                return NotFound();
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
            List<CommentDto> commentsDtos = new List<CommentDto>();
            foreach (var comment in comments)
            {
                CommentDto commentDto = _mapper.Map<CommentDto>(comment);
                commentsDtos.Add(commentDto);
            }
            return Ok(commentsDtos);
        }
    }
}
