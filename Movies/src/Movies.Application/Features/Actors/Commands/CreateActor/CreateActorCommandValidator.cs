using FluentValidation;
using Movies.Application.Validators;

namespace Movies.Application.Features.Actors.Commands.CreateActor;

public class CreateActorCommandValidator : AbstractValidator<CreateActorCommand>
{
	public CreateActorCommandValidator()
	{
		RuleFor(p => p.Name)
			.NotEmpty().WithMessage("{PropertyName} is required.")
			.MaximumLength(120).WithMessage("{PropertyName} must not exceed 120 characters.")
			.WithName("Name");

		RuleFor(p => p.Birthdate)
			.NotEmpty().WithMessage("{PropertyName} is required.");

		RuleFor(p => p.Photo)
			.Must(BeAValidSize).WithMessage("The photo must be less than 4MB");

		When(p => p.PhotoFileType != null, () =>
		{
			RuleFor(p => p.PhotoFileType!)
				.SetValidator(new ImageFileTypeValidator());
		});
	}

	private bool BeAValidSize(Stream? photo)
	{
		if (photo == null)
		{
			return true;
		}

		var sizeInMb = photo.Length / 1024f / 1024f;
		return sizeInMb <= 4;
	}
}
