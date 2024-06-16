using Movies.Domain.Entities;

namespace Movies.Application.Features.Actors.Queries.GetActors;

public record GetActorsQueryResponse
{
	public GetActorsQueryResponse(int id, string name)
	{
		Id = id;
		Name = name;
	}

	public GetActorsQueryResponse(Actor actor)
	{
		Id = actor.Id;
		Name = actor.Name;
	}

	public int Id { get; private set; }
	public string Name { get; private set; } = null!;
}
