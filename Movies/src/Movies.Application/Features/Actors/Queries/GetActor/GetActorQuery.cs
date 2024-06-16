using MediatR;
using Movies.Application.Responses;

namespace Movies.Application.Features.Actors.Queries.GetActor;

public record GetActorQuery(int Id) :IRequest<ObjectResponse<GetActorQueryResponse>>;
