using MediatR;
using Movies.Application.Contracts.Persistence;
using Movies.Application.Exceptions;
using Movies.Domain.Entities;

namespace Movies.Application.Features.Genres.Commands.UpdateGenre;

public class UpdateGenreCommandHandler(IGenreRepository genreRepository)
	: IRequestHandler<UpdateGenreCommand, Unit>
{
	public async Task<Unit> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
	{
		var genreToUpdate = await genreRepository.GetByIdAsync(request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Genre), request.Id);

		var validator = new UpdateGenreCommandValidator(genreRepository);
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (validationResult.Errors.Count > 0)
			throw new ValidatorException(validationResult);

		genreToUpdate.Name = request.Name;
		genreRepository.Update(genreToUpdate);
		await genreRepository.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
