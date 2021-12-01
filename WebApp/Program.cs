var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

var app = builder.Build();
app.UseStaticFiles();

app.MapControllerRoute("default", "{controller=Post}/{action=Index}/{id?}");

app.Run();
