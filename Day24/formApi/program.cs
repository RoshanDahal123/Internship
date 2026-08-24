

using formApi.FormApp.Application;
using formApi.FormApp.Infrastructure.SqlRepo;
using formApi.FormApp.Infrastructure.Services;
using FormApp.Application;
using FormApp.Infrastructure.SqlRepo;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Each layer registers itself — Program.cs just composes them.
builder.Services.AddApplication();
builder.Services.AddSqlRepoInfrastructure(builder.Configuration);
builder.Services.AddServicesInfrastructure(options =>
{
    options.UploadRootPath = Path.Combine(builder.Environment.WebRootPath, "uploads");
    options.RequestPathPrefix = "/uploads";
});

// CORS — required or your React dev server (localhost:5173/3000) gets
// blocked by the browser when calling this API from a different port
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // adjust to your actual frontend dev URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles(); // serves the uploaded files from the wwwroot folder

app.Run();
