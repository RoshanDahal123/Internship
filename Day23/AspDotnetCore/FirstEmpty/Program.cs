var builder = WebApplication.CreateBuilder(args);

//registering the services

builder.Services.AddControllersWithViews();
var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapGet("/", () => "Hello World!");

app.Run();
