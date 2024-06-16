namespace Movies.Application.Responses.Storage;

public record FileResponse
{
	public string? Url { get; internal set; }

	public FileResponse()
	{
		Url = null;
	}

	public FileResponse(string url)
	{
		Url = url;
	}

	public void SetUrl(string url)
	{
		Url = url;
	}
};
