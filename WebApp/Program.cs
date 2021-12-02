using WebApp.Helper;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddTransient<SiteHelper>();

var app = builder.Build();
app.UseStaticFiles();
app.MapControllerRoute("dashboard", "{area:exists}/{controller=category}/{action=index}/{id?}");
app.MapControllerRoute("default", "{controller=Post}/{action=Index}/{id?}");
app.Run();
