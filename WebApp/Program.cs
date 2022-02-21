using Microsoft.AspNetCore.Authentication.Cookies;
using WebApp.Interfaces;
using WebApp.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/account/login";
    option.AccessDeniedPath = "/account/denied";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(30); // 5 minutes    
});

var app = builder.Build();
app.UseExceptionHandler("/Error");
app.UseStatusCodePagesWithRedirects("/Error/{0}");
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
//app.MapControllerRoute("default", "{controller=Post}/{action=Index}/{id?}");
app.MapControllerRoute("manage", "{area:exists}/{controller=Home}/{action=index}/{id?}");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");


app.Run();
