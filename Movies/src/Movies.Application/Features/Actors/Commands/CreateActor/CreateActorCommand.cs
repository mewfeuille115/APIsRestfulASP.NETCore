using MediatR;
using Movies.Application.Responses;

namespace Movies.Application.Features.Actors.Commands.CreateActor;

public record CreateActorCommand(
	string Name,
	DateTime Birthdate,
	Stream? Photo,
	string? PhotoFileType
) : IRequest<ObjectResponse<CreateActorCommandResponse>>;
