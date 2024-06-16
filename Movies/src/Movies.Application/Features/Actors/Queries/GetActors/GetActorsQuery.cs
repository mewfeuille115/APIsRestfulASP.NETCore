using MediatR;
using Movies.Application.Pages;
using Movies.Application.Responses;

namespace Movies.Application.Features.Actors.Queries.GetActors;

public record GetActorsQuery(PageDto Page) : IRequest<ListResponse<GetActorsQueryResponse>>;
