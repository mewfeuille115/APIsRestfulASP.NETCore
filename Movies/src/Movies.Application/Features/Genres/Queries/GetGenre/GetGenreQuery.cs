using MediatR;
using Movies.Application.Responses;

namespace Movies.Application.Features.Genres.Queries.GetGenre;

public record GetGenreQuery(int Id) : IRequest<ObjectResponse<GenreResponse>>;
