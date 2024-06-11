namespace Movies.API.Responses;

public class LinkBase
{
	public string Href { get; set; } = string.Empty;
	public string Rel { get; set; } = string.Empty;
	public string Method { get; set; } = string.Empty;
}
