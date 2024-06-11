namespace Movies.Application.Responses;

public record ObjectResponse<T> : BaseResponse
{
	public ObjectResponse(T data)
	{
		Data = data;
		Success = true;
	}

	public ObjectResponse(T data, string message)
	{
		Data = data;
		Message = message;
		Success = true;
	}

	public ObjectResponse(T data, string message, bool success)
	{
		Data = data;
		Message = message;
		Success = success;
	}

    public ObjectResponse(T data, string message, bool success, List<string> validationError)
    {
		Data = data;
		Message = message;
		Success = success;
		ValidationErrors = validationError;
    }

    public T Data { get; private set; }
}
