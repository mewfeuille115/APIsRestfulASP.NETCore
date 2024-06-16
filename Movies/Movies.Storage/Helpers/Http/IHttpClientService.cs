namespace Movies.Storage.Helpers.Http;

public interface IHttpClientService
{
	Task<HttpResponseMessage> DeleteAsync(string requestUri);
	Task<HttpResponseMessage> GetAsync(string requestUri);
	Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
}
