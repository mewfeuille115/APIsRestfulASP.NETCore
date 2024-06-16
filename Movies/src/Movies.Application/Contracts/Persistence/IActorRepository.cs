using Movies.Domain.Entities;

namespace Movies.Application.Contracts.Persistence;

public interface IActorRepository : IBaseRepository<Actor, int>
{
}
