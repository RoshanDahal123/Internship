using formApi.FormApp.Application.Interfaces;
using formApi.FormApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FormApp.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IFormEntryService, FormEntryService>();
		return services;
	}
}
