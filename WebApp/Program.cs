using WebApp.Helper;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddTransient<SiteProvider>();

var app = builder.Build();
app.UseRouting();
app.UseStaticFiles();
//app.MapControllerRoute("default", "{controller=Post}/{action=Index}/{id?}");
app.MapControllerRoute("dashboard", "{area:exists}/{controller=category}/{action=index}/{id?}");
app.MapControllerRoute("default", "{controller=Post}/{action=Index}/{id?}");
app.Run();
