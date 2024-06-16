using MediatR;
using Movies.Application.Contracts.Persistence;
using Movies.Application.Contracts.Storage;
using Movies.Application.Exceptions;
using Movies.Application.Responses;
using Movies.Application.Responses.Storage;
using Movies.Domain.Entities;

namespace Movies.Application.Features.Actors.Commands.CreateActor;

public class CreateActorCommandHandler(
		IActorRepository actorRepository,
		IStorageService storageService)
	: IRequestHandler<CreateActorCommand, ObjectResponse<CreateActorCommandResponse>>
{
	public async Task<ObjectResponse<CreateActorCommandResponse>> Handle(CreateActorCommand request, CancellationToken cancellationToken)
	{
		var validator = new CreateActorCommandValidator();
		var validatorResult = await validator.ValidateAsync(request, cancellationToken);
		if (validatorResult.Errors.Count > 0)
			throw new ValidatorException(validatorResult);

		FileResponse fileResponse = new();
		if (request.Photo is not null)
		{
			var resultStorage = await storageService.SaveAsync(request.Photo);
			fileResponse.SetUrl(resultStorage.Url!);
		}

		var actorToCreate = new Actor
		{
			Name = request.Name,
			Birthdate = request.Birthdate,
			PhotoUrl = fileResponse.Url,
		};

		actorToCreate = await actorRepository.AddAsync(actorToCreate, string.Empty, cancellationToken);
		await actorRepository.SaveChangesAsync(cancellationToken);

		var data = new CreateActorCommandResponse(
			actorToCreate!.Id,
			actorToCreate!.Name,
			actorToCreate!.Birthdate,
			actorToCreate!.PhotoUrl
		);

		return new ObjectResponse<CreateActorCommandResponse>(data);
	}
}
