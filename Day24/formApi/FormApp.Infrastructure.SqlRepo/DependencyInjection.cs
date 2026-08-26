using formApi.FormApp.Application.Interfaces;
using formApi.FormApp.Infrastructure.SqlRepo.Repositories;
using formApi.FormApp.Infrastructure.SqlRepo.Persistence;
using Microsoft.EntityFrameworkCore;


namespace formApi.FormApp.Infrastructure.SqlRepo;

// Keeps all "how do I wire this layer up" logic next to the layer itself,
// so Program.cs in the API project stays a one-liner per layer.
public static class DependencyInjection
{
    public static IServiceCollection AddSqlRepoInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IFormEntryRepository, FormEntryRepository>();
        services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }
}
