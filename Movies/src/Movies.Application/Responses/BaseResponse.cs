namespace Movies.Application.Responses;

public record BaseResponse
{
	public BaseResponse()
	{
		Success = true;
	}

	public BaseResponse(string message)
	{
		Message = message;
		Success = true;
	}

	public BaseResponse(string message, bool success)
	{
		Message = message;
		Success = success;
	}

	public BaseResponse(string message, bool success, List<string> validationErrors)
	{
		Message = message;
		Success = success;
		ValidationErrors = validationErrors;
	}

	public bool Success { get; internal set; }
	public string Message { get; internal set; } = string.Empty;
	public List<string> ValidationErrors { get; internal set; } = [];

	public void SetSuccess(bool success)
	{
		Success = success;
	}

	public void SetMessage(string message)
	{
		Message = message;
	}

	public void SetValidationErrors(List<string> validationErrors)
	{
		ValidationErrors = validationErrors;
	}
}
