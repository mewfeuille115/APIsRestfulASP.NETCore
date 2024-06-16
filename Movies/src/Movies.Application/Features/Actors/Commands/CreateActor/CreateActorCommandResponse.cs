namespace Movies.Application.Features.Actors.Commands.CreateActor;

public record CreateActorCommandResponse(
	int Id,
	string Name,
	DateTime Birthdate,
	string? PhotoUrl
);
