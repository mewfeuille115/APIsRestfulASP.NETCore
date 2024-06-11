using Movies.Application.Features.Genres.Queries.GetGenres;
using System.Text.Json.Serialization;

namespace Movies.API.Responses.V1.Genre;

public record GetGenreListResponse : GenreListResponse
{
	public GetGenreListResponse(GenreListResponse original) : base(original)
	{
	}

	[JsonPropertyOrder(order: 100)]
	public List<LinkBase>? Links { get; set; }
}
