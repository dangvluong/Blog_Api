using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DataTransferObject;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : BaseController
    {
        private readonly int pageSize = 12;
        public PostController(IRepositoryManager repository, IMapper mapper) : base(repository, mapper)
        {

        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<ListPostDto>> GetPosts([FromQuery] int page = 1)
        {
            var posts = await _repository.Post.GetActivePosts(page, pageSize, trackChanges: false);
            if (posts == null)
                return NotFound();
            ListPostDto listPost = new ListPostDto
            {
                Posts = MapPosts(posts),
                TotalPage = await _repository.Post.CountTotalPage(pageSize, p => p.IsActive == true && p.IsDeleted == false)
            };
            return Ok(listPost);
        }
        [HttpGet("fromcategory/{categoryid}")]
        public async Task<ActionResult<ListPostDto>> GetPostsFromCategory(int categoryId, [FromQuery] int page = 1)
        {
            var posts = await _repository.Post.GetPostsFromCategory(categoryId,page, pageSize, trackChanges: false);
            if (posts == null)
                return NotFound();
            ListPostDto listPost = new ListPostDto
            {
                Posts = MapPosts(posts),
                TotalPage = await _repository.Post.CountTotalPage(pageSize, p => p.CategoryId == categoryId && p.IsActive == true && p.IsDeleted == false)
            };
            return Ok(listPost);
        }

        [HttpGet("managergetposts")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<ListPostDto>> ManagerGetPosts([FromQuery] int page = 1)
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
            Post post = await _repository.Post.GetPostById(id, trackChanges, countView);
            if (post == null)
                return NotFound();
            if (post.IsDeleted)
                return BadRequest("Bài viết đã bị xóa");
            PostDto postDto = _mapper.Map<PostDto>(post);
            return Ok(postDto);
        }

        // PUT: api/Post/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult<int>> PostPost(Post post)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(post.Thumbnail))
                    post.Thumbnail = "/images/thumbnails/default.jpg";
                _repository.Post.AddPost(post);
                return await _repository.SaveChanges();
            }
            return BadRequest();
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _repository.Post.GetPostById(id, trackChanges: true);
            if (post == null)
            {
                return NotFound("Không tìm thấy bài viết");
            }
            if (post.IsDeleted)
            {
                return BadRequest("Bài viết đã bị xóa trước đó");
            }
            _repository.Post.DeletePost(post);
            await _repository.SaveChanges();
            return NoContent();
        }

        [HttpPost("restore/{id}")]
        [Authorize]
        public async Task<IActionResult> RestorePost(int id)
        {
            var post = await _repository.Post.GetPostById(id, trackChanges: true);
            if (post == null)
            {
                return NotFound("Không tìm thấy bài viết");
            }
            if (!post.IsDeleted)
            {
                return BadRequest("Bài viết chưa bị xóa trước đó");
            }
            _repository.Post.RestorePost(post);
            await _repository.SaveChanges();
            return NoContent();
        }

        [HttpGet("getpostsbymember/{id}")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsByMember(int id)
        {
            bool includeInActivePosts = false;
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (User.Identity.IsAuthenticated && int.Parse(currentUserId) == id)
                includeInActivePosts = true;
            IEnumerable<Post> posts = await _repository.Post.GetPostsByMember(id, trackChanges: false, includeInActivePosts);
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
        [HttpPost("approve/{postId}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Approve(int postId)
        {
            Post post = await _repository.Post.GetPostById(postId, trackChanges: true);
            if (post == null)
                return NotFound();
            if (post.IsActive)
                return BadRequest("Bài viết đã được phê duyệt trước đó");
            post.IsActive = true;
            await _repository.SaveChanges();
            return NoContent();
        }

        [HttpPost("removeapproved/{postId}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> RemoveApproved(int postId)
        {
            Post post = await _repository.Post.GetPostById(postId, trackChanges: true);
            if (post == null)
                return NotFound();
            if (!post.IsActive)
                return BadRequest("Bài viết chưa được phê duyệt trước đó");
            post.IsActive = false;
            await _repository.SaveChanges();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<ListPostDto>> Search(string keyword, int page = 1)
        {
            if (string.IsNullOrEmpty(keyword))
                return BadRequest();
            var posts = await _repository.Post.Search(keyword, page, pageSize, trackChanges: false);
            if (posts == null)
                return NotFound();
            ListPostDto listPost = new ListPostDto
            {
                Posts = MapPosts(posts),
                TotalPage = await _repository.Post.CountTotalPage(pageSize, p => p.Title.Contains(keyword))
            };
            return Ok(listPost);
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
