using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController
    {
        public CommentController(RepositoryManager repository) : base(repository)
        {
        }


        // GET: api/Comment
        [HttpGet]
        public async Task<IEnumerable<Comment>> GetComments()
        {
            return await _repository.Comment.GetComments();
        }

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<Comment> GetComment(int id)
        {
            return await _repository.Comment.GetComment(id);            
        }

        // GET: api/GetCommentsByPost/5
        [HttpGet]
        [Route("GetCommentsByPost/{id}")]
        public async Task<IEnumerable<Comment>> GetCommentsByPost(int id)
        {
            return await _repository.Comment.GetCommentsByPost(id);            
        }

        // PUT: api/Comment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Comment.Update(comment);
            await _repository.SaveChanges();
            return NoContent();
        }

        // POST: api/Comment        
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Comment.Add(comment);            
            await _repository.SaveChanges();
            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
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
            _repository.Comment.Delete(comment);            
            await _repository.SaveChanges();
            return NoContent();
        }
       
    }
}
