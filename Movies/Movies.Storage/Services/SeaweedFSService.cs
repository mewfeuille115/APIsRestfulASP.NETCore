using Microsoft.Extensions.Options;
using Movies.Application.Contracts.Storage;
using Movies.Application.Exceptions;
using Movies.Application.Responses.Storage;
using Movies.Storage.Configurations;
using Movies.Storage.Helpers.Http;
using Movies.Storage.Responses;
using System.Net.Http.Json;

namespace Movies.Storage.Services;

public class SeaweedFSService(
		IOptionsMonitor<SeaweedFSConfiguration> optionsMonitor,
		IHttpClientService httpClientService)
	: IStorageService
{
	private readonly SeaweedFSConfiguration _fSConfiguration = optionsMonitor.CurrentValue;

	public async Task<FileResponse> SaveAsync(Stream File)
	{
		var seaweedFSBaseResponse = await httpClientService.GetAsync($"{_fSConfiguration.BaseUrl}/dir/assign");
		var data = await seaweedFSBaseResponse.Content.ReadFromJsonAsync<GetBaseSeaweedFSResponse>();
		if (!seaweedFSBaseResponse.IsSuccessStatusCode)
			throw new BadRequestException("Error to get the information of SeaweedFS Service.");

		var fileUrl = $"{_fSConfiguration.PublicUrl}/{data!.FId}";
		using var content = new MultipartFormDataContent
		{
			{ new StreamContent(File), "file" }
		};
		var postResponse = await httpClientService.PostAsync(fileUrl, content);
		if (!postResponse.IsSuccessStatusCode)
			throw new BadRequestException("Error to upload the file to SeaweedFS Service.");

		return new FileResponse(fileUrl);
	}

	public async Task<FileResponse> UpdateAsync(Stream File, string? url)
	{
		if (url is null)
		{
			var createResponse = await SaveAsync(File);
			url = createResponse.Url
				?? throw new BadRequestException("Error to upload the file to SeaweedFS Service.");
		}

		using var content = new MultipartFormDataContent
		{
			{ new StreamContent(File), "file" }
		};
		var postResponse = await httpClientService.PostAsync(url, content);
		if (!postResponse.IsSuccessStatusCode)
			throw new BadRequestException("Error to upload the file to SeaweedFS Service.");

		return new FileResponse(url);
	}

	public async Task DeleteAsync(string url)
	{
		var response = await httpClientService.DeleteAsync(url);
		if (!response.IsSuccessStatusCode)
			throw new BadRequestException("Error to delete the file to SeaweedFS Service.");
	}
}
