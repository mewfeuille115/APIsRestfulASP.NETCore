using MediatR;
using Movies.Application.Contracts.Persistence;
using Movies.Application.Exceptions;
using Movies.Application.Responses;
using Movies.Domain.Entities;

namespace Movies.Application.Features.Actors.Queries.GetActor;

public class GetActorQueryHandler(IActorRepository actorRepository)
	: IRequestHandler<GetActorQuery, ObjectResponse<GetActorQueryResponse>>
{
	public async Task<ObjectResponse<GetActorQueryResponse>> Handle(GetActorQuery request, CancellationToken cancellationToken)
	{
		var actor = await actorRepository.GetByIdAsync(request.Id, cancellationToken)
			?? throw new NotFoundException(nameof(Actor), request.Id);

		var data = new GetActorQueryResponse(
			actor.Id,
			actor.Name,
			actor.Birthdate,
			actor.PhotoUrl
		);

		return new ObjectResponse<GetActorQueryResponse>(data);
	}
}
