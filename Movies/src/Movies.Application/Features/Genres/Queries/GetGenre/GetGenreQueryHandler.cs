using MediatR;
using Movies.Application.Contracts.Persistence;
using Movies.Application.Exceptions;
using Movies.Application.Responses;
using Movies.Domain.Entities;

namespace Movies.Application.Features.Genres.Queries.GetGenre;

public class GetGenreQueryHandler(IGenreRepository genreRepository)
	: IRequestHandler<GetGenreQuery, ObjectResponse<GenreResponse>>
{
	public async Task<ObjectResponse<GenreResponse>> Handle(GetGenreQuery request, CancellationToken cancellationToken)
	{
		var genre = await genreRepository.GetByIdAsync(request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Genre), request.Id);

		var data = new GenreResponse(genre.Id, genre.Name);

		return new ObjectResponse<GenreResponse>(data);
	}
}
