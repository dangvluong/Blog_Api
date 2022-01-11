using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : BaseController
    {       

        public PostController(IRepositoryManager repository):base(repository)
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
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            Post post = await _repository.Post.GetPost(id);
            if (post == null)
                return NotFound();
            return Ok(post); 
        }

        // PUT: api/Post/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> UpdatePost(Post post)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            _repository.Post.UpdatePost(post);
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
                _repository.Post.AddPost(post);
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
            //post.IsDeleted = true;
            _repository.Post.DeletePost(post);
            await _repository.SaveChanges();

            return NoContent();
        }
        [HttpGet("getpostsbymember/{id}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByMember(int id)
        {
            IEnumerable<Post> posts = await _repository.Post.GetPostsByMember(id);
            if (posts == null)
                return NotFound();
            return Ok(posts);
        }      
    }
}
