using Movies.Application.Responses;
using System.Text.Json.Serialization;

namespace Movies.API.Responses;

public record LinkResponse<T> : ObjectResponse<T>
{
	public LinkResponse(
		T data, string message, bool success, List<string> validationError)
		: base(data, message, success, validationError)
	{
	}

	[JsonPropertyOrder(order: 100)]
	public List<LinkBase>? Links { get; set; }
}
