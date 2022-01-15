using Microsoft.AspNetCore.Mvc;
using WebApi.Dto;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController
    {
        public CommentController(IRepositoryManager repository) : base(repository)
        {
        }


        // GET: api/Comment
        //[HttpGet]
        //public async Task<IEnumerable<Comment>> GetComments()
        //{
        //    return await _repository.Comment.GetComments();
        //}

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            Comment comment = await _repository.Comment.GetComment(id);
            if (comment == null)
                return NotFound();
            return Ok(comment);
        }

        // GET: api/GetCommentsByPost/5
        [HttpGet]
        [Route("GetCommentsByPost/{id}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByPost(int id)
        {
            IEnumerable<CommentDto> comments = await _repository.Comment.GetCommentsByPost(id);
            if (comments == null)
                return NotFound();
            return Ok(comments);
        }

        // PUT: api/Comment/5
        [HttpPut("{id}")]
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
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _repository.Comment.GetComment(id);
            if (comment == null)
            {
                return NotFound();
            }
            _repository.Comment.DeleteComment(comment);            
            await _repository.SaveChanges();
            return NoContent();
        }
       
    }
}
