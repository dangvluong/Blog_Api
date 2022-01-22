using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataTransferObject;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : BaseController
    {
        private readonly int pageSize = 12;        
        public PostController(IRepositoryManager repository, IMapper mapper) : base(repository,mapper)
        {
            
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<ListPostDto>> GetPosts([FromQuery] int page = 1)
        {
            var posts = await _repository.Post.GetPosts(page, pageSize, trackChanges: false);
            if (posts == null)
                return NotFound();
            ListPostDto listPost = new ListPostDto
            {
                Posts = MapPosts(posts),
                TotalPage = await _repository.Post.CountTotalPage(pageSize)
            };
            return Ok(listPost);
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(int id, [FromQuery] bool countView = false)
        {
            bool trackChanges = countView ? true : false;
            Post post = await _repository.Post.GetPost(id, trackChanges, countView);
            if (post == null)
                return NotFound();
            PostDto postDto = _mapper.Map<PostDto>(post);
            return Ok(postDto);
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
            return NoContent();
        }

        // POST: api/Post
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<int>> PostPost(Post post)
        {
            if (ModelState.IsValid)
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
            var post = await _repository.Post.GetPost(id, trackChanges: false);
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
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsByMember(int id)
        {
            IEnumerable<Post> posts = await _repository.Post.GetPostsByMember(id, trackChanges: false);
            if (posts == null)
                return NotFound();
            return Ok(MapPosts(posts));
        }
        [HttpGet("gettrendingpost")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetTrendingPost()
        {
            var posts = await _repository.Post.GetTrendingPost();
            if (posts == null)
                return NotFound();
            return Ok(MapPosts(posts));
        }

        [HttpGet("getmostrecentposts")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetMostRecentPosts()
        {
            var posts = await _repository.Post.GetMostRecentPosts();
            if (posts == null)
                return NotFound();            
            return Ok(MapPosts(posts));
        }

        [HttpGet("gettodayhighlightposts")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetTodayHighlightPosts()
        {
            var posts = await _repository.Post.GetTodayHighlightPosts();
            if (posts == null)
                return NotFound();
            
            return Ok(MapPosts(posts));
        }
        [HttpGet("getfeaturedposts")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetFeaturedPosts()
        {
            var posts = await _repository.Post.GetFeaturedPosts();
            if (posts == null)
                return NotFound();            
            return Ok(MapPosts(posts));
        }
        private List<PostDto> MapPosts(IEnumerable<Post> posts)
        {
            List<PostDto> postDtos = new List<PostDto>();
            foreach (var post in posts)
            {
                postDtos.Add(_mapper.Map<PostDto>(post));
            }
            return postDtos;
        }
    }
}
