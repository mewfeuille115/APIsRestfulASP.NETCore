using MediatR;
using Movies.Application.Contracts.Persistence;
using Movies.Application.Contracts.Storage;
using Movies.Application.Exceptions;
using Movies.Application.Responses.Storage;
using Movies.Domain.Entities;

namespace Movies.Application.Features.Actors.Commands.UpdateActor;

public class UpdateActorCommandHandler(
		IActorRepository actorRepository,
		IStorageService storageService)
	: IRequestHandler<UpdateActorCommand, Unit>
{
	public async Task<Unit> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
	{
		var actorToUpdate = await actorRepository.GetByIdAsync(request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Actor), request.Id);

		var validator = new UpdateActorCommandValidator();
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (validationResult.Errors.Count > 0)
			throw new ValidatorException(validationResult);

		FileResponse fileResponse = new();
		if (request.Photo is not null)
		{
			var resultStorage = await storageService.UpdateAsync(request.Photo, actorToUpdate.PhotoUrl);
			fileResponse.SetUrl(resultStorage.Url!);
		}

		actorToUpdate.Name = request.Name;
		actorToUpdate.Birthdate = request.Birthdate;
		actorToUpdate.PhotoUrl = fileResponse.Url;
		actorRepository.Update(actorToUpdate, string.Empty);
		await actorRepository.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
