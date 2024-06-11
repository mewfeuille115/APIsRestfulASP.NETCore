using Microsoft.EntityFrameworkCore;
using Movies.Application.Contracts.Persistence;
using Movies.Domain.Entities;

namespace Movies.Persistence.Repositories;

public class GenreRepository : BaseRepository<Genre, int>, IGenreRepository
{
	public GenreRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
	}

	public async Task<bool> IsGenreNameUnique(string name)
	{
		return !await _dbContext.Genres.AnyAsync(x => x.Name.Equals(name));
	}
}
