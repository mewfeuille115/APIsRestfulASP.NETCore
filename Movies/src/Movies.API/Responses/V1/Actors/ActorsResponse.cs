using Movies.Application.Features.Actors.Queries.GetActors;
using System.Text.Json.Serialization;

namespace Movies.API.Responses.V1.Actors;

public record ActorsResponse : GetActorsQueryResponse
{
	public ActorsResponse(GetActorsQueryResponse original) : base(original)
	{
	}

	[JsonPropertyOrder(order: 100)]
	public List<LinkBase>? Links { get; set; }
}
