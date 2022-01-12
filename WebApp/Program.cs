using Microsoft.AspNetCore.Authentication.Cookies;
using WebApp.Interfaces;
using WebApp.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/member/login";
    option.AccessDeniedPath = "/member/denied";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(5); // 5 minutes
});

var app = builder.Build();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
//app.MapControllerRoute("default", "{controller=Post}/{action=Index}/{id?}");
app.MapControllerRoute("dashboard", "{area:exists}/{controller=category}/{action=index}/{id?}");
app.MapControllerRoute("default", "{controller=Post}/{action=Index}/{id?}");


app.Run();
