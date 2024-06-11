using MediatR;
using Movies.Application.Responses;

namespace Movies.Application.Features.Genres.Commands.CreateGenre;

public record CreateGenreCommand(string Name) : IRequest<ObjectResponse<CreateGenreCommandResponse>>;
