using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CommentController : BaseController
    {
        public CommentController(IRepositoryManager repository) : base(repository)
        {
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostComment(Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            string token = User.FindFirst(ClaimTypes.Authentication).Value;
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            comment.AuthorId = userId;
            comment.DateCreate = DateTime.UtcNow;            
            int result = await _repository.Comment.PostComment(comment, token);
            if (result >= 0)
                return RedirectToAction("Detail", "Post",new {Id = comment.PostId}, "comments");
            return BadRequest();
        }
    }
}
