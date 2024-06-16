using MediatR;
using Movies.Application.Contracts.Persistence;
using Movies.Application.Responses;
using System.Collections.Immutable;

namespace Movies.Application.Features.Actors.Queries.GetActors;

public class GetActorsQueryHandler(IActorRepository actorRepository)
	: IRequestHandler<GetActorsQuery, ListResponse<GetActorsQueryResponse>>
{
	public async Task<ListResponse<GetActorsQueryResponse>> Handle(GetActorsQuery request, CancellationToken cancellationToken)
	{
		var actors = await actorRepository.ListAllAsync(request.Page, cancellationToken);
		var data = actors.Select(actor => new GetActorsQueryResponse(actor))
			.ToImmutableList();

		return new ListResponse<GetActorsQueryResponse>(data);
	}
}
