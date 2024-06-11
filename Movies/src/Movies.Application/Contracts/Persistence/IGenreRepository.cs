using Movies.Domain.Entities;

namespace Movies.Application.Contracts.Persistence;

public interface IGenreRepository : IBaseRepository<Genre, int>
{
	Task<bool> IsGenreNameUnique(string name);
}
