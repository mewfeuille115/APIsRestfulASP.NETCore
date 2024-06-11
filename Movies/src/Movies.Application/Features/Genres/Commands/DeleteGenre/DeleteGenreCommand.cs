using MediatR;

namespace Movies.Application.Features.Genres.Commands.DeleteGenre;

public record DeleteGenreCommand(int Id) : IRequest<Unit>;
