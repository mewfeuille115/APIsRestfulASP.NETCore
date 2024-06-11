using FluentValidation.Results;

namespace Movies.Application.Exceptions;

public class ValidatorException : Exception
{
	public List<string> ValidationErrors { get; internal set; }

	public ValidatorException(ValidationResult validationResult)
	{
		ValidationErrors = [];

		foreach (var validationError in validationResult.Errors)
			ValidationErrors.Add(validationError.ErrorMessage);
	}

	public ValidatorException(List<string> validatorException)
	{
		ValidationErrors = validatorException;
	}
}
