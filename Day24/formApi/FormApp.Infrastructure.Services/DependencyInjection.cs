
using formApi.FormApp.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace formApi.FormApp.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServicesInfrastructure(this IServiceCollection services, Action<FileStorageOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddScoped<IFileStorageService, FileStorageService>();
        return services;
    }
}
