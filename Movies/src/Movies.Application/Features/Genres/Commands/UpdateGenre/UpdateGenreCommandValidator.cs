using FluentValidation;
using Movies.Application.Contracts.Persistence;

namespace Movies.Application.Features.Genres.Commands.UpdateGenre;

public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
{
	private readonly IGenreRepository _genreRepository;

	public UpdateGenreCommandValidator(IGenreRepository genreRepository)
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

	private async Task<bool> GenreNameUnique(UpdateGenreCommand e, CancellationToken token)
	{
		return await _genreRepository.IsGenreNameUnique(e.Name);
	}
}
