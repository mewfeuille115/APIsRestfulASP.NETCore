namespace Movies.Application.Features.Actors.Queries.GetActor;

public record GetActorQueryResponse(
	int Id,
	string Name,
	DateTime Birthdate,
	string? PhotoUrl
);
