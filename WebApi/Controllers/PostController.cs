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
    public class PostController : BaseController
    {       

        public PostController(RepositoryManager repository):base(repository)
        {
            
        }

        // GET: api/Post
        [HttpGet]
        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _repository.Post.GetPosts();
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<Post> GetPost(int id)
        {
            return  await _repository.Post.GetPost(id);            
        }

        // PUT: api/Post/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> UpdatePost(Post post)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Post.Update(post);
            await _repository.SaveChanges();
            return Ok();
        }

        // POST: api/Post
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<int>> PostPost(Post post)
        {
            if(ModelState.IsValid)
            {
                _repository.Post.Add(post);
                return await _repository.SaveChanges();                
            }
            return BadRequest();            
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _repository.Post.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }
            post.IsDeleted = true;
            //__repository.Posts.Remove(post);
            await _repository.SaveChanges();

            return NoContent();
        }
        [HttpGet("getpostsbymember/{id}")]
        public async Task<IList<Post>> GetPostsByMember(int id)
        {
            return await _repository.Post.GetPostsByMember(id);
        }      
    }
}
