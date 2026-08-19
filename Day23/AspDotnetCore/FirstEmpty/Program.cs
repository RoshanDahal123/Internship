var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.Use((context,next) =>
{
    context.Response.WriteAsync("Text from the middleware1");
    return next();
});
app.Use((context, next) =>
{
    context.Response.WriteAsync("Text from the middleware2");
    return next();
});
app.Run((context) => context.Response.WriteAsync("Response from Middleware Run"));
app.MapGet("/", () => "Hello World!");

app.Run();
