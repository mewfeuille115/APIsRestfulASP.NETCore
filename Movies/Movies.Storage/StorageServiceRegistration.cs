using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Contracts.Storage;
using Movies.Storage.Configurations;
using Movies.Storage.Helpers.Http;
using Movies.Storage.Services;

namespace Movies.Storage;

public static class StorageServiceRegistration
{
	public static IServiceCollection AddStorageServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<SeaweedFSConfiguration>(configuration.GetSection("SeaweedFSConfiguration"));
		services.AddHttpClient<IHttpClientService, HttpClientService>();
		services.AddTransient<IStorageService, SeaweedFSService>();

		return services;
	}
}
