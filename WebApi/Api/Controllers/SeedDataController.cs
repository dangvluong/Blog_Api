using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Helper;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedDataController : BaseController
    {
        private AppDbContext _context;
        public SeedDataController(IRepositoryManager repository, AppDbContext context) : base(repository)
        {
            _context = context;
        }

        [HttpGet("cleardata")]
        public async Task<bool> ClearData()
        {
            return await _context.Database.EnsureDeletedAsync();
        }
        [HttpGet("migrate")]
        public async Task Migrate()
        {
            await _context.Database.MigrateAsync();
        }
        [HttpGet("seeddata")]
        public async Task SeedData()
        {
            //seed roles
            var roles = new Role[]
            {
                new Role{Name = "Admin", CanChange = false,ColorDisplay="badge-danger"},
                new Role{Name = "Moderator",CanChange  =false,ColorDisplay="badge-primary"},
                new Role{Name = "Member",CanChange = false,ColorDisplay="badge-secondary"}
            };
            //context.Roles.Add(new Role
            //{
            //    Name = "admin1"
            //});
            _context.Roles.AddRange(roles);

            await _repository.SaveChanges();
            //seed member
            var members = new Member[]
            {
                new Member{
                Username = "vanluong92",
                Password = SiteHelper.HashPassword("123123"),
                Gender = true,
                DateOfBirth = DateTime.Now,
                Email = "admin@gmail.com",
                FullName = "admin",
                DateCreate = DateTime.Now,
                AvatarUrl = DefaultValue.Avatar,
                AboutMe = DefaultValue.About
                },
                new Member{
                Username = "vanluong93",
                Password = SiteHelper.HashPassword("123123"),
                Gender = true,
                DateOfBirth = DateTime.Now,
                Email = "admin1@gmail.com",
                FullName = "admin1",
                DateCreate = DateTime.Now,
                AvatarUrl = DefaultValue.Avatar,
                AboutMe = DefaultValue.About
                },
                new Member{
                Username = "vanluong94",
                Password = SiteHelper.HashPassword("123123"),
                Gender = true,
                DateOfBirth = DateTime.Now,
                Email = "admin2@gmail.com",
                FullName = "admin2",
                DateCreate = DateTime.Now,
                AvatarUrl = DefaultValue.Avatar,
                AboutMe = DefaultValue.About
                },
                new Member{
                Username = "vanluong95",
                Password = SiteHelper.HashPassword("123123"),
                Gender = false,
                DateOfBirth = DateTime.Now,
                Email = "admin3@gmail.com",
                FullName = "admin3",
                DateCreate = DateTime.Now,
                AvatarUrl = DefaultValue.Avatar,
                AboutMe = DefaultValue.About
                },
                new Member{
                Username = "string",
                Password = SiteHelper.HashPassword("string"),
                Gender = false,
                DateOfBirth = DateTime.Now,
                Email = "admin4@gmail.com",
                FullName = "admin4",
                DateCreate = DateTime.Now,
                AvatarUrl = DefaultValue.Avatar,
                AboutMe = DefaultValue.About
                }
            };
            foreach (var member in members)
            {
                member.Roles = new List<Role>();
                member.Roles.Add(await _repository.Role.GetRoleByName("Admin", trackChanges: true));
                member.Roles.Add(await _repository.Role.GetRoleByName("Member", trackChanges: true));
            }
            _context.Members.AddRange(members);
            await _repository.SaveChanges();
            //seed posts and categories
            await SeedPostAndCategoryAsync();
            await SeedComments();
            //await context.SaveChangesAsync();
        }

        private async Task SeedComments()
        {
            var fk = new Faker<Comment>();
            fk.RuleFor(p => p.Content, f => $"Comment " + f.Lorem.Sentences(5));
            fk.RuleFor(p => p.DateCreate, f => f.Date.Between(new DateTime(2019, 12, 31), new DateTime(2021, 12, 31)));
            var members = (await _repository.Member.GetMembers(trackChanges: true)).ToList();
            var posts = (await _repository.Post.GetPosts(1, 50, trackChanges: true)).ToList();
            var rand = new Random();
            var comments = new List<Comment>();
            for (int i = 0; i < 30; i++)
            {
                var comment = fk.Generate();
                comment.AuthorId = members[rand.Next(members.Count)].Id;
                comment.PostId = posts[0].Id;
                comments.Add(comment);
            }
            _context.Comments.AddRange(comments);
            await _repository.SaveChanges();
            //comments.Clear();
            //var commentsInDb = _context.Comments.ToList();
            //add comments child
            //for (int i = 0; i < 30; i++)
            //{
            //    var commentParent = commentsInDb[rand.Next(commentsInDb.Count)];
            //    var commentChild = fk.Generate();
            //    commentChild.AuthorId = members[rand.Next(members.Count)].Id;
            //    commentChild.PostId = commentParent.PostId;
            //    commentChild.CommentParentId = commentParent.Id;
            //    comments.Add(commentChild);
            //}
            //_context.Comments.AddRange(comments);
            //await _repository.SaveChanges();
        }

        private async Task SeedPostAndCategoryAsync()
        {
            var fakerCategory = new Faker<Category>();
            int j = 1;
            fakerCategory.RuleFor(c => c.Name, name => $"Category {j++}: " + name.Lorem.Sentence(1, 2).Trim('.'));
            var category1 = fakerCategory.Generate();
            var category11 = fakerCategory.Generate();
            var category2 = fakerCategory.Generate();
            var category3 = fakerCategory.Generate();
            var category31 = fakerCategory.Generate();
            var category4 = fakerCategory.Generate();
            category11.ParentCategory = category1;
            category31.ParentCategory = category3;
            var categories = new Category[] { category1, category11, category2, category3, category31, category4 };
            _repository.Category.AddRange(categories);
            await _repository.SaveChanges();
            var fakerPost = new Faker<Post>();
            var index = 1;
            var members = await _context.Members.ToListAsync();
            var rand = new Random();
            fakerPost.RuleFor(p => p.Title, fk => $"Post {index++} " + fk.Lorem.Sentence(3, 4).Trim('.'));            
            fakerPost.RuleFor(p => p.Content, fk => fk.Lorem.Paragraphs(10));
            fakerPost.RuleFor(p => p.DateCreated, fk => fk.Date.Between(new DateTime(2019, 1, 1), new DateTime(2021, 12, 31)));

            var posts = new List<Post>();
            var categoriesInDb = await _context.Categories.ToListAsync();
            for (int i = 0; i < 50; i++)
            {
                var post = fakerPost.Generate();
                post.Thumbnail = "/images/thumbnails/default.jpg";
                post.CategoryId = categoriesInDb[rand.Next(categoriesInDb.Count)].Id;
                post.AuthorId = members[rand.Next(members.Count)].Id;
                posts.Add(post);
            }
            _context.Posts.AddRange(posts);
            await _repository.SaveChanges();
        }
    }
}
