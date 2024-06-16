using Movies.Domain.Entities;

namespace Movies.Application.Features.Genres.Queries.GetGenres;

public record GetGenresQueryResponse
{
	public GetGenresQueryResponse(int id, string name)
	{
		Id = id;
		Name = name;
	}

	public GetGenresQueryResponse(Genre genre)
	{
		Id = genre.Id;
		Name = genre.Name;
	}

	public int Id { get; private set; }
	public string Name { get; private set; }
}
