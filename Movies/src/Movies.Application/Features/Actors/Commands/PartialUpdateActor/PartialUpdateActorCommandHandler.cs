using MediatR;
using Movies.Application.Contracts.Persistence;
using Movies.Application.Exceptions;
using Movies.Domain.Entities;

namespace Movies.Application.Features.Actors.Commands.PartialUpdateActor;

public class PartialUpdateActorCommandHandler(
		IActorRepository actorRepository)
	: IRequestHandler<PartialUpdateActorCommand, Unit>
{
	public async Task<Unit> Handle(PartialUpdateActorCommand request, CancellationToken cancellationToken)
	{
		var actorToUpdate = await actorRepository.GetByIdAsync(request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Actor), request.Id);

		actorToUpdate.Name = request.Name ?? actorToUpdate.Name;
		actorToUpdate.Birthdate = request.Birthdate ?? actorToUpdate.Birthdate;
		actorRepository.Update(actorToUpdate, string.Empty);
		await actorRepository.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
