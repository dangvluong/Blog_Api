using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedDataController : BaseController
    {
        public SeedDataController(AppDbContext context) : base(context)
        {
        }
        [HttpGet("cleardata")]
        public async Task<bool> ClearData()
        {
            var result = await context.Database.EnsureDeletedAsync();
            return result;
        }
        [HttpGet("migrate")]
        public async Task Migrate()
        {
            await context.Database.MigrateAsync();
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
            context.Roles.AddRange(roles);
            context.SaveChanges();
            var roles1 = context.Roles.ToList();
            //seed member
            Member member = new Member()
            {
                Username = "vanluong92",
                Password = SiteHelper.HashPassword("123123"),
                Gender = true,
                DateOfBirth = DateTime.Now,
                Email = "admin@gmail.com",
                FullName = "admin",
                DateCreate = DateTime.Now
            };
            member.Roles = new List<Role>();
            var role = context.Roles.FirstOrDefault(s => s.Name == "admin");
            member.Roles.Add(context.Roles.FirstOrDefault(s => s.Name == "admin"));
            member.Roles.Add(context.Roles.FirstOrDefault(s => s.Name == "member"));
            context.Members.Add(member);
            await context.SaveChangesAsync();
            //seed posts and categories
            await SeedPostAndCategoryAsync();
            await context.SaveChangesAsync();
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
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
            var fakerPost = new Faker<Post>();
            var index = 1;
            fakerPost.RuleFor(p => p.Title, fk => $"Post {index++} " + fk.Lorem.Sentence(3, 4).Trim('.'));
            fakerPost.RuleFor(p => p.Description, fk => fk.Lorem.Sentences(3));
            fakerPost.RuleFor(p => p.Content, fk => fk.Lorem.Paragraphs(10));
            fakerPost.RuleFor(p => p.DateCreated, fk => fk.Date.Between(new DateTime(2021, 12, 31), new DateTime(2019, 1, 1)));
            fakerPost.RuleFor(p => p.AuthorId, fk => context.Members.FirstOrDefault(m => m.Username == "vanluong92").Id);

            var posts = new List<Post>();
            categories = await context.Categories.ToArrayAsync();
            var rand = new Random();
            for (int i = 0; i < 50; i++)
            {
                var post = fakerPost.Generate();
                post.CategoryId = categories[rand.Next(categories.Length)].Id;
                posts.Add(post);
            }
            context.Posts.AddRange(posts);
            await context.SaveChangesAsync();
        }        
    }
}
