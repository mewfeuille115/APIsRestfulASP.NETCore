using Movies.Domain.Entities;

namespace Movies.Application.Features.Genres.Queries.GetGenres;

public record GenreListResponse
{
	public GenreListResponse(int id, string name)
	{
		Id = id;
		Name = name;
	}

	public GenreListResponse(Genre genre)
	{
		Id = genre.Id;
		Name = genre.Name;
	}

	public int Id { get; private set; }
	public string Name { get; private set; }
}
