using FluentValidation;
using System.Net.Mime;

namespace Movies.Application.Validators;

public class ImageFileTypeValidator : AbstractValidator<string>
{
	public ImageFileTypeValidator()
	{
		RuleFor(p => p)
			.Must(x =>
				x.Equals("image/jpg")
				|| x.Equals(MediaTypeNames.Image.Jpeg)
				|| x.Equals("image/png")
				|| x.Equals("image/gif"))
			.WithMessage("The image must be a file of type jpeg, png or gif.")
			.WithName("ContentType");
	}
}
