﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Helpers;
using Movies.API.Responses;
using Movies.API.Responses.V1.Genre;
using Movies.Application.Features.Genres.Commands.CreateGenre;
using Movies.Application.Features.Genres.Commands.DeleteGenre;
using Movies.Application.Features.Genres.Commands.UpdateGenre;
using Movies.Application.Features.Genres.Queries.GetGenre;
using Movies.Application.Features.Genres.Queries.GetGenres;
using Movies.Application.Pages;
using Movies.Application.Responses;

namespace Movies.API.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class GenresController(
	IMediator mediator,
	HypermediaLinkService linkService) : ControllerBase
{
	/// <summary>
	/// Consult the list of genres.
	/// </summary>
	/// <returns></returns>
	[HttpGet(Name = "GetGenres")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListLinkResponse<GetGenreListResponse>))]
	public async Task<ActionResult<ListLinkResponse<GetGenreListResponse>>> Get([FromQuery] PageDto pageDto)
	{
		var result = await mediator.Send(new GetGenresQuery(pageDto));

		var resultWithLinks = result.Data.Select(genre => new GetGenreListResponse(genre)
		{
			Links =
			[
				linkService.AddSelfLink("GetGenre", new { genre.Id }),
				linkService.AddUpdateLink("UpdateGenre", "genre"),
				linkService.AddDeleteLink("DeleteGenre", new { genre.Id }, "genre"),
			],
		});

		var listResponse = new ListResponse<GetGenreListResponse>(resultWithLinks.ToList());

		var listResponseWithLinks = new ListLinkResponse<GetGenreListResponse>(listResponse)
		{
			Links =
			[
				linkService.AddCreateLink("CreateGenre", "genre"),
			],
		};

		return Ok(listResponseWithLinks);
	}

	/// <summary>
	/// Consult a single genre by Id.
	/// </summary>
	/// <param name="id">Id of the genre.</param>
	/// <returns></returns>
	[HttpGet("{id:int}", Name = "GetGenre")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LinkResponse<GenreResponse>))]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponse))]
	public async Task<ActionResult<LinkResponse<GenreResponse>>> Get([FromRoute] int id)
	{
		var result = await mediator.Send(new GetGenreQuery(id));

		var response = new LinkResponse<GenreResponse>(
			result.Data, result.Message, result.Success, result.ValidationErrors)
		{
			Links =
			[
				linkService.AddSelfLink("GetGenre", new { result.Data!.Id }),
				linkService.AddUpdateLink("UpdateGenre", "genre"),
				linkService.AddDeleteLink("DeleteGenre", new { result.Data.Id }, "genre"),
			],
		};

		return Ok(response);
	}

	/// <summary>
	/// Create a new genre.
	/// </summary>
	/// <param name="createGenreCommand">Genre to create.</param>
	/// <returns></returns>
	[HttpPost(Name = "CreateGenre")]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LinkResponse<CreateGenreCommandResponse>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
	public async Task<ActionResult> Post([FromBody] CreateGenreCommand createGenreCommand)
	{
		var result = await mediator.Send(createGenreCommand);

		var response = new LinkResponse<CreateGenreCommandResponse>(
			result.Data, result.Message, result.Success, result.ValidationErrors)
		{
			Links =
			[
				linkService.AddSelfLink("GetGenre", new { result.Data!.Id }),
				linkService.AddUpdateLink("UpdateGenre", "genre"),
				linkService.AddDeleteLink("DeleteGenre", new { result.Data.Id }, "genre"),
			],
		};

		return new CreatedAtRouteResult("GetGenre", new { id = result.Data.Id }, response);
	}

	/// <summary>
	/// Update a exist genre.
	/// </summary>
	/// <param name="updateGenreCommand">Genre to update.</param>
	/// <returns></returns>
	[HttpPut(Name = "UpdateGenre")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponse))]
	public async Task<ActionResult> Put([FromBody] UpdateGenreCommand updateGenreCommand)
	{
		await mediator.Send(updateGenreCommand);

		return new NoContentResult();
	}

	/// <summary>
	/// Delete a genre by Id.
	/// </summary>
	/// <param name="id">Id of the genre to delete.</param>
	/// <returns></returns>
	[HttpDelete("{id:int}", Name = "DeleteGenre")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponse))]
	public async Task<ActionResult> Delete([FromRoute] int id)
	{
		await mediator.Send(new DeleteGenreCommand(id));

		return new NoContentResult();
	}
}
