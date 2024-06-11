using Movies.Application.Responses;
using System.Text.Json.Serialization;

namespace Movies.API.Responses;

public record ListLinkResponse<T> : ListResponse<T>
{
	public ListLinkResponse(ListResponse<T> original) : base(original)
	{
	}

	[JsonPropertyOrder(order: 100)]
	public List<LinkBase>? Links { get; set; }
}
