using MediatR;

namespace Movies.Application.Features.Actors.Commands.DeleteActor;

public record DeleteActorCommand(int Id) : IRequest<Unit>;
