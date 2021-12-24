using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedDataController : BaseController
    {
        private AppDbContext _context;
        public SeedDataController(RepositoryManager repository, AppDbContext context) : base(repository)
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
                new Role{Name = "admin"},
                new Role{Name = "member"}
            };
            //context.Roles.Add(new Role
            //{
            //    Name = "admin1"
            //});
            _repository.Role.AddRange(roles);            
            await _repository.SaveChanges();
            var roles1 = _repository.Role.GetRoles();
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
                DateCreate = DateTime.Now
                },
                new Member{
                Username = "vanluong93",
                Password = SiteHelper.HashPassword("123123"),
                Gender = true,
                DateOfBirth = DateTime.Now,
                Email = "admin1@gmail.com",
                FullName = "admin1",
                DateCreate = DateTime.Now
                },
                new Member{
                Username = "vanluong94",
                Password = SiteHelper.HashPassword("123123"),
                Gender = true,
                DateOfBirth = DateTime.Now,
                Email = "admin2@gmail.com",
                FullName = "admin2",
                DateCreate = DateTime.Now
                },
                new Member{
                Username = "vanluong95",
                Password = SiteHelper.HashPassword("123123"),
                Gender = false,
                DateOfBirth = DateTime.Now,
                Email = "admin3@gmail.com",
                FullName = "admin3",
                DateCreate = DateTime.Now
                },
                new Member{
                Username = "vanluong96",
                Password = SiteHelper.HashPassword("123123"),
                Gender = false,
                DateOfBirth = DateTime.Now,
                Email = "admin4@gmail.com",
                FullName = "admin4",
                DateCreate = DateTime.Now
                }
            };
            foreach (var member in members)
            {
                member.Roles = new List<Role>();                
                member.Roles.Add(await _repository.Role.GetRoleByName("admin"));
                member.Roles.Add(await _repository.Role.GetRoleByName("member"));                
            }
            _repository.Member.AddRange(members);            
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
            var members = await _repository.Member.GetMembers();
            var posts = await _repository.Post.GetPosts();
            var rand = new Random();
            var comments = new List<Comment>();
            for (int i = 0; i < posts.Count; i++)
            {
                var comment = fk.Generate();
                comment.AuthorId = members[rand.Next(members.Count)].Id;
                comment.PostId = posts[i].Id;
                comments.Add(comment);
            }
            _repository.Comment.AddRange(comments);            
            await _repository.SaveChanges();
            comments.Clear();
            var commentsInDb = await _repository.Comment.GetComments();
            //add comments child
            for (int i = 0; i < 30; i++)
            {
                var commentParent = commentsInDb[rand.Next(commentsInDb.Count)];
                var commentChild = fk.Generate();
                commentChild.AuthorId = members[rand.Next(members.Count)].Id;
                commentChild.PostId = commentParent.PostId;
                commentChild.CommentParentId = commentParent.Id;
                comments.Add(commentChild);
            }
            _repository.Comment.AddRange(comments);            
            await _repository.SaveChanges();
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
            var members = await _repository.Member.GetMembers();
            var rand = new Random();
            fakerPost.RuleFor(p => p.Title, fk => $"Post {index++} " + fk.Lorem.Sentence(3, 4).Trim('.'));
            fakerPost.RuleFor(p => p.Description, fk => fk.Lorem.Sentences(3));
            fakerPost.RuleFor(p => p.Content, fk => fk.Lorem.Paragraphs(10));
            fakerPost.RuleFor(p => p.DateCreated, fk => fk.Date.Between(new DateTime(2019, 1, 1), new DateTime(2021, 12, 31)));           

            var posts = new List<Post>();
            var categoriesInDb = await _repository.Category.GetCategories();            
            for (int i = 0; i < 50; i++)
            {
                var post = fakerPost.Generate();
                post.CategoryId = categoriesInDb[rand.Next(categoriesInDb.Count)].Id;
                post.AuthorId = members[rand.Next(members.Count)].Id;
                posts.Add(post);
            }
            _repository.Post.AddRange(posts);            
            await _repository.SaveChanges();
        }
    }
}
