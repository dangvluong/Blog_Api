using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using WebApp.Interfaces;
using WebApp.Repositories;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
Log.Information("Starting up....");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((ctx, lc) => lc
       .WriteTo.Console()
       .ReadFrom.Configuration(ctx.Configuration));
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
    app.UseSerilogRequestLogging();
    app.UseAuthentication();
    app.UseAuthorization();
    //app.MapControllerRoute("default", "{controller=Post}/{action=Index}/{id?}");
    app.MapControllerRoute("manage", "{area:exists}/{controller=Home}/{action=index}/{id?}");
    app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");


    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
