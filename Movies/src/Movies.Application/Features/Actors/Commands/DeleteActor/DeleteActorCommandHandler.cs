using MediatR;
using Movies.Application.Contracts.Persistence;
using Movies.Application.Contracts.Storage;
using Movies.Application.Exceptions;
using Movies.Domain.Entities;

namespace Movies.Application.Features.Actors.Commands.DeleteActor;

public class DeleteActorCommandHandler(
		IActorRepository actorRepository,
		IStorageService storageService)
	: IRequestHandler<DeleteActorCommand, Unit>
{
	public async Task<Unit> Handle(DeleteActorCommand request, CancellationToken cancellationToken)
	{
		var actorToDelete = await actorRepository.GetByIdAsync(request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Actor), request.Id);

		if (actorToDelete.PhotoUrl is not null) 
			await storageService.DeleteAsync(actorToDelete.PhotoUrl);
        
		actorRepository.Delete(actorToDelete);
		await actorRepository.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
