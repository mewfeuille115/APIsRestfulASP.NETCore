namespace Movies.Storage.Helpers.Http;

public class HttpClientService(
	HttpClient httpClient) : IHttpClientService
{
	public Task<HttpResponseMessage> DeleteAsync(string requestUri)
	{
		return httpClient.DeleteAsync(requestUri);
	}

	public Task<HttpResponseMessage> GetAsync(string requestUri)
	{
		return httpClient.GetAsync(requestUri);
	}

	public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
	{
		return await httpClient.PostAsync(requestUri, content);
	}
}
