using Movies.Application.Contracts.Persistence;
using Movies.Domain.Entities;

namespace Movies.Persistence.Repositories;

public class ActorRepository : BaseRepository<Actor, int>, IActorRepository
{
	public ActorRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
	}
}
