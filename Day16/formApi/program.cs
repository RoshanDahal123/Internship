
using formApi.Data;
using Microsoft.EntityFrameworkCore;

var builder= WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDBContext>(options=>
 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


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
app.UseStaticFiles();// server the uploaded files from the wwwroot folder

app.Run();