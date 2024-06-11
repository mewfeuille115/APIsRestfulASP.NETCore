using MediatR;
using Movies.Application.Pages;
using Movies.Application.Responses;

namespace Movies.Application.Features.Genres.Queries.GetGenres;

public record GetGenresQuery(PageDto Page) : IRequest<ListResponse<GenreListResponse>>;
