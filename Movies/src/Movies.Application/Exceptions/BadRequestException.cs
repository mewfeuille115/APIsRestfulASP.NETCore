namespace Movies.Application.Exceptions;

public class BadRequestException(string message = "Invalid request, please try again later.")
	: Exception(message)
{
}
