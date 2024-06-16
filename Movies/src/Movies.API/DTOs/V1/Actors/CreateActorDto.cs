namespace Movies.API.DTOs.V1.Actors;

public record CreateActorDto(
	string Name,
	DateTime Birthdate,
	IFormFile? Photo
);