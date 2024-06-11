using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Contracts.Persistence;
using Movies.Persistence.Repositories;

namespace Movies.Persistence;

public static class PersistenceServiceRegistration
{
	public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

		services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));

		services.AddScoped<IGenreRepository, GenreRepository>();

		return services;
	}
}
