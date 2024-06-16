namespace Movies.API.DTOs.V1.Actors;

public record UpdateActorDto(
	int Id,
	string Name,
	DateTime Birthdate,
	IFormFile? Photo
);