using Movies.Application.Exceptions;
using Movies.Application.Responses;
using System.Net;
using System.Text.Json;

namespace Movies.API.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
	public async Task Invoke(HttpContext context)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			await ConvertException(context, ex, logger);
		}
	}

	private static Task ConvertException(HttpContext context, Exception exception, ILogger logger)
	{
		HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

		context.Response.ContentType = "application/json";

		var result = new BaseResponse();
		result.SetSuccess(false);

		switch (exception)
		{
			case ValidatorException validationException:
				httpStatusCode = HttpStatusCode.BadRequest;
				result.SetValidationErrors(validationException.ValidationErrors);
				break;
			case BadRequestException badRequestException:
				httpStatusCode = HttpStatusCode.BadRequest;
				result.SetMessage(badRequestException.Message);
				break;
			case NotFoundException:
				httpStatusCode = HttpStatusCode.NotFound;
				break;
			case TaskCanceledException:
				result.SetMessage("Request cancelled.");
				break;
			case Exception:
				httpStatusCode = HttpStatusCode.BadRequest;
				break;
		}

		context.Response.StatusCode = (int)httpStatusCode;

		if (string.IsNullOrEmpty(result.Message) && result.ValidationErrors.Count == 0)
		{
			result.SetMessage("Invalid request, please try again later.");
			logger.LogError(exception, "Error: {Message}", result.Message);
		}

		return context.Response.WriteAsync(JsonSerializer.Serialize(result));
	}
}
