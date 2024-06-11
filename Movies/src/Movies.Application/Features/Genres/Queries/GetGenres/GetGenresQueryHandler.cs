using MediatR;
using Movies.Application.Contracts.Persistence;
using Movies.Application.Responses;
using System.Collections.Immutable;

namespace Movies.Application.Features.Genres.Queries.GetGenres;

public class GetGenresQueryHandler(IGenreRepository genreRepository)
	: IRequestHandler<GetGenresQuery, ListResponse<GenreListResponse>>
{
	public async Task<ListResponse<GenreListResponse>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
	{
		var genres = await genreRepository.ListAllAsync(request.Page, cancellationToken);
		var data = genres.Select(genre => new GenreListResponse(genre))
			.ToImmutableList();

		return new ListResponse<GenreListResponse>(data);
	}
}
