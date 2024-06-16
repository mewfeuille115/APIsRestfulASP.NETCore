using MediatR;

namespace Movies.Application.Features.Actors.Commands.UpdateActor;

public record UpdateActorCommand(
	int Id,
	string Name,
	DateTime Birthdate,
	Stream? Photo,
	string? PhotoFileType
) : IRequest<Unit>;
