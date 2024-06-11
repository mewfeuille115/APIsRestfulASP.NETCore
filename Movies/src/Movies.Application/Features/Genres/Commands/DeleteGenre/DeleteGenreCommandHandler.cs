using MediatR;
using Movies.Application.Contracts.Persistence;
using Movies.Application.Exceptions;
using Movies.Domain.Entities;

namespace Movies.Application.Features.Genres.Commands.DeleteGenre;

public class DeleteGenreCommandHandler(IGenreRepository genreRepository)
	: IRequestHandler<DeleteGenreCommand, Unit>
{
	public async Task<Unit> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
	{
		var genteToDelete = await genreRepository.GetByIdAsync(request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Genre), request.Id);
		genreRepository.Delete(genteToDelete);
		await genreRepository.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
