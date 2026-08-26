using formApi.FormApp.Application.Interfaces;
using formApi.FormApp.Application.Services;

using Microsoft.Extensions.DependencyInjection;

namespace formApi.FormApp.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IFormEntryService, FormEntryService>();
		services.AddScoped<IAdminAuthService, AdminAuthService>();
		return services;
	}
}
