using Microsoft.EntityFrameworkCore;

using Serilog;
using TodoApp.Data;
using TodoApp.Services;
using Microsoft.OpenApi; 
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);



        //---SERILOG CONFIGURATION-----

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            //.Enrich.WithMachineName()
            //.Enrich.WithThreadId()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
            .WriteTo.File("logs/todoapp-.txt",
                   rollingInterval: RollingInterval.Day,
                   outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} " +
                   "{Properties:j}{NewLine}{Exception}")
            .CreateLogger();
        builder.Host.UseSerilog();



        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddControllers(); //for API


        ////Database (InMemory)
        //builder.Services.AddDbContext<AppDbContext>(options =>
        //    options.UseInMemoryDatabase("TodoDb"));

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConn")));

        // Dependency Injection
        builder.Services.AddScoped<ITodoService, TodoService>();


        // Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo

            {
                Title = "Todo API",
                Version = "v1",
                Description = "A simple Todo API built with ASP.NET Core"
            });
        });
        var app = builder.Build();


        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API v1");
                c.InjectStylesheet("/swagger-custom.css");
            });
        }

        app.UseSerilogRequestLogging(); // log HTTP requests
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.UseStaticFiles(); 

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapControllers();

        // Instead of EnsureCreated(), we will use migrations.
        // But for development, you can still use EnsureCreated() if you prefer,
        // but migrations are the standard way.
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }



        app.Run();
    }
}