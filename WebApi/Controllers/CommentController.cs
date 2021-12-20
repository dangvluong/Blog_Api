using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController
    {
        public CommentController(AppDbContext context) : base(context)
        {
        }

        //private readonly AppDbContext _context;

        //public CommentController(AppDbContext context)
        //{
        //    _context = context;
        //}

        // GET: api/Comment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await context.Comments.ToListAsync();
        }

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        // GET: api/GetCommentsByPost/5
        [HttpGet]
        [Route("GetCommentsByPost/{id}")]
        public async Task<IList<Comment>> GetCommentsByPost(int id)
        {           
            return await context.Comments.Where(c => c.PostId == id).ToListAsync(); 
        }

        // PUT: api/Comment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            context.Comments.Update(comment);
            await context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Comment        
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            context.Comments.Add(comment);
            await context.SaveChangesAsync();
            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        // DELETE: api/Comment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return context.Comments.Any(e => e.Id == id);
        }
    }
}
