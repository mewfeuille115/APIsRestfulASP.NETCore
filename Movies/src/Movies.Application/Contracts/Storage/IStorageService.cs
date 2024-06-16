using Movies.Application.Responses.Storage;

namespace Movies.Application.Contracts.Storage;

public interface IStorageService
{
	Task<FileResponse> SaveAsync(Stream File);
	Task<FileResponse> UpdateAsync(Stream File, string? url);
	Task DeleteAsync(string url);
}
