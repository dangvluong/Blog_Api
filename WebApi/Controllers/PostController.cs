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
        public PostController(IRepositoryManager repository) : base(repository)
        {

        }

        // GET: api/Post
        [HttpGet]
        public async Task<ListPostDto> GetPosts([FromQuery] int page = 1)
        {
            ListPostDto listPost = new ListPostDto
            {
                Posts = await _repository.Post.GetPosts(page, pageSize, trackChanges: false),
                TotalPage = await _repository.Post.CountTotalPage(pageSize)
            };

            return listPost;
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id, [FromQuery] bool countView = false)
        {
            bool trackChanges = countView ? true : false;
            Post post = await _repository.Post.GetPost(id, trackChanges, countView);
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
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByMember(int id)
        {
            IEnumerable<Post> posts = await _repository.Post.GetPostsByMember(id, trackChanges: false);
            if (posts == null)
                return NotFound();
            return Ok(posts);
        }
        [HttpGet("gettrendingpost")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetTrendingPost()
        {
            var posts = await _repository.Post.GetTrendingPost();
            if (posts == null)
                return NotFound();

            List<PostDto> postDto = new List<PostDto>();
            foreach (var post in posts)
            {
                postDto.Add(new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Description = post.Description,
                    Content = post.Content,
                    DateCreated = post.DateCreated,
                    DateModifier = post.DateModifier,
                    IsActive = post.IsActive,
                    IsDeleted = post.IsDeleted,
                    CategoryId = post.CategoryId,
                    Category = post.Category,
                    AuthorId = post.AuthorId,
                    Author = new MemberDto
                    {
                        Id = post.Author.Id,
                        Username = post.Author.Username,
                        Gender = post.Author.Gender,
                        FullName = post.Author.FullName,
                        Email = post.Author.Email,
                        DateCreate = post.Author.DateCreate,
                        DateOfBirth = post.Author.DateOfBirth,
                        IsActive = post.Author.IsActive,
                        IsBanned = post.Author.IsBanned
                    },
                    Comments = post.Comments,
                    CountView = post.CountView
                });
            }
            return Ok(postDto);


        }
        [HttpGet("getmostrecentposts")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetMostRecentPosts()
        {
            var posts = await _repository.Post.GetMostRecentPosts();
            if (posts == null)
                return NotFound();

            List<PostDto> postDto = new List<PostDto>();
            foreach (var post in posts)
            {
                postDto.Add(new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Description = post.Description,
                    Content = post.Content,
                    DateCreated = post.DateCreated,
                    DateModifier = post.DateModifier,
                    IsActive = post.IsActive,
                    IsDeleted = post.IsDeleted,
                    CategoryId = post.CategoryId,
                    Category = post.Category,
                    AuthorId = post.AuthorId,
                    Author = new MemberDto
                    {
                        Id = post.Author.Id,
                        Username = post.Author.Username,
                        Gender = post.Author.Gender,
                        FullName = post.Author.FullName,
                        Email = post.Author.Email,
                        DateCreate = post.Author.DateCreate,
                        DateOfBirth = post.Author.DateOfBirth,
                        IsActive = post.Author.IsActive,
                        IsBanned = post.Author.IsBanned
                    },
                    Comments = post.Comments,
                    CountView = post.CountView
                });
            }
            return Ok(postDto);
        }

        [HttpGet("gettodayhighlightposts")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetTodayHighlightPosts()
        {
            var posts = await _repository.Post.GetTodayHighlightPosts();
            if (posts == null)
                return NotFound();

            List<PostDto> postDto = new List<PostDto>();
            foreach (var post in posts)
            {
                postDto.Add(new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Description = post.Description,
                    Content = post.Content,
                    DateCreated = post.DateCreated,
                    DateModifier = post.DateModifier,
                    IsActive = post.IsActive,
                    IsDeleted = post.IsDeleted,
                    CategoryId = post.CategoryId,
                    Category = post.Category,
                    AuthorId = post.AuthorId,
                    Author = new MemberDto
                    {
                        Id = post.Author.Id,
                        Username = post.Author.Username,
                        Gender = post.Author.Gender,
                        FullName = post.Author.FullName,
                        Email = post.Author.Email,
                        DateCreate = post.Author.DateCreate,
                        DateOfBirth = post.Author.DateOfBirth,
                        IsActive = post.Author.IsActive,
                        IsBanned = post.Author.IsBanned
                    },
                    Comments = post.Comments,
                    CountView = post.CountView
                });
            }
            return Ok(postDto);
        }
        [HttpGet("getfeaturedposts")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetFeaturedPosts()
        {
            var posts = await _repository.Post.GetFeaturedPosts();
            if (posts == null)
                return NotFound();

            List<PostDto> postDto = new List<PostDto>();
            foreach (var post in posts)
            {
                postDto.Add(new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Description = post.Description,
                    Content = post.Content,
                    DateCreated = post.DateCreated,
                    DateModifier = post.DateModifier,
                    IsActive = post.IsActive,
                    IsDeleted = post.IsDeleted,
                    CategoryId = post.CategoryId,
                    Category = post.Category,
                    AuthorId = post.AuthorId,
                    Author = new MemberDto
                    {
                        Id = post.Author.Id,
                        Username = post.Author.Username,
                        Gender = post.Author.Gender,
                        FullName = post.Author.FullName,
                        Email = post.Author.Email,
                        DateCreate = post.Author.DateCreate,
                        DateOfBirth = post.Author.DateOfBirth,
                        IsActive = post.Author.IsActive,
                        IsBanned = post.Author.IsBanned
                    },
                    Comments = post.Comments,
                    CountView = post.CountView
                });
            }
            return Ok(postDto);
        }

    }
}
