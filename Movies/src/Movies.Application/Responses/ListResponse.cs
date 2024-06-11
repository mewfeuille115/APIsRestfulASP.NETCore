namespace Movies.Application.Responses;

public record ListResponse<T> : BaseResponse
{
	public int Count { get; private set; }

	public ListResponse(IEnumerable<T> data, string message = "List found")
	{
		Count = data.Count();
		Data = data;
		Message = message;
		Success = true;
	}

	public ListResponse(IEnumerable<T> data, string message, bool success)
	{
		Count = data.Count();
		Data = data;
		Success = success;
		Message = message;
	}

	public ListResponse(IEnumerable<T> data, string message, bool success, List<string> validationErrors)
	{
		Count = data.Count();
		Data = data;
		Message = message;
		Success = success;
		ValidationErrors = validationErrors;
	}

	public IEnumerable<T> Data { get; private set; } = [];
}
