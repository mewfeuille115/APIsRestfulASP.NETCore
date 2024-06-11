using MediatR;
using Movies.Application.Contracts.Persistence;
using Movies.Application.Exceptions;
using Movies.Application.Responses;
using Movies.Domain.Entities;

namespace Movies.Application.Features.Genres.Commands.CreateGenre;

public class CreateGenreCommandHandler(IGenreRepository genreRepository)
	: IRequestHandler<CreateGenreCommand, ObjectResponse<CreateGenreCommandResponse>>
{
	public async Task<ObjectResponse<CreateGenreCommandResponse>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
	{
		var validator = new CreateGenreCommandValidator(genreRepository);
		var validatorResult = await validator.ValidateAsync(request, cancellationToken);
		if (validatorResult.Errors.Count > 0)
			throw new ValidatorException(validatorResult);

		var genreToCreate = new Genre { Name = request.Name };
		genreToCreate = await genreRepository.AddAsync(genreToCreate, cancellationToken);
		await genreRepository.SaveChangesAsync(cancellationToken);

		var data = new CreateGenreCommandResponse(genreToCreate!.Id, genreToCreate!.Name);

		return new ObjectResponse<CreateGenreCommandResponse>(data);
	}
}
