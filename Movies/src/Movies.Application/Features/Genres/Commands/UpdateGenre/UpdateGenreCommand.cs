using MediatR;

namespace Movies.Application.Features.Genres.Commands.UpdateGenre;

public record UpdateGenreCommand(int Id, string Name) : IRequest<Unit>;
