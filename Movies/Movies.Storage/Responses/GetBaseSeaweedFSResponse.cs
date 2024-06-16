namespace Movies.Storage.Responses;

public class GetBaseSeaweedFSResponse
{
	public string FId { get; set; } = null!;
	public string Url { get; set; } = null!;
	public string PublicUrl { get; set; } = null!;
	public int Count { get; set; }
}
