using FluentValidation;
using Movies.Application.Contracts.Persistence;

namespace Movies.Application.Features.Genres.Commands.CreateGenre;

public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
{
	private readonly IGenreRepository _genreRepository;

	public CreateGenreCommandValidator(IGenreRepository genreRepository)
	{
		_genreRepository = genreRepository;

		RuleFor(p => p.Name)
			.NotEmpty().WithMessage("{PropertyName} is required.")
			.NotNull().WithMessage("{PropertyName} is required.")
			.WithName("Name");

		RuleFor(e => e)
			.MustAsync(GenreNameUnique)
			.WithMessage("An genre with the same name already exists.");
	}

	private async Task<bool> GenreNameUnique(CreateGenreCommand e, CancellationToken token)
	{
		return await _genreRepository.IsGenreNameUnique(e.Name);
	}
}
