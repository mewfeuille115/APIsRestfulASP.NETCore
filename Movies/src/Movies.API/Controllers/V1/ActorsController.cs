using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Movies.API.DTOs.V1.Actors;
using Movies.API.Helpers;
using Movies.API.Responses;
using Movies.API.Responses.V1.Actors;
using Movies.Application.Features.Actors.Commands.CreateActor;
using Movies.Application.Features.Actors.Commands.DeleteActor;
using Movies.Application.Features.Actors.Commands.PartialUpdateActor;
using Movies.Application.Features.Actors.Commands.UpdateActor;
using Movies.Application.Features.Actors.Queries.GetActor;
using Movies.Application.Features.Actors.Queries.GetActors;
using Movies.Application.Pages;
using Movies.Application.Responses;

namespace Movies.API.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class ActorsController(
	IMediator mediator,
	HypermediaLinkService linkService) : ControllerBase
{
	/// <summary>
	/// Consult the list of actors.
	/// </summary>
	/// <returns></returns>
	[HttpGet(Name = "GetActors")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ListLinkResponse<ActorsResponse>))]
	public async Task<IActionResult> Get([FromQuery] PageDto pageDto)
	{
		var result = await mediator.Send(new GetActorsQuery(pageDto));

		var resultWithLinks = result.Data.Select(actor => new ActorsResponse(actor)
		{
			Links =
			[
				linkService.AddSelfLink("GetActor", new { actor.Id }),
				linkService.AddUpdateLink("UpdateActor", "actor"),
				linkService.AddPartialUpdateLink("PartialUpdateActor", new { actor.Id }, "actor"),
				linkService.AddDeleteLink("DeleteActor", new { actor.Id }, "actor"),
			],
		});

		var listResponse = new ListResponse<ActorsResponse>(resultWithLinks.ToList());

		var listResponseWithLinks = new ListLinkResponse<ActorsResponse>(listResponse)
		{
			Links =
			[
				linkService.AddCreateLink("CreateActor", "actor"),
			],
		};

		return Ok(listResponseWithLinks);
	}

	/// <summary>
	/// Consult a single actor by Id.
	/// </summary>
	/// <param name="id">Id of the actor.</param>
	/// <returns></returns>
	[HttpGet("{id:int}", Name = "GetActor")]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LinkResponse<GetActorQueryResponse>))]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponse))]
	public async Task<IActionResult> Get([FromRoute] int id)
	{
		var result = await mediator.Send(new GetActorQuery(id));

		var response = new LinkResponse<GetActorQueryResponse>(
			result.Data, result.Message, result.Success, result.ValidationErrors)
		{
			Links =
			[
				linkService.AddSelfLink("GetActor", new { result.Data!.Id }),
				linkService.AddUpdateLink("UpdateActor", "actor"),
				linkService.AddPartialUpdateLink("PartialUpdateActor", new { result.Data.Id }, "actor"),
				linkService.AddDeleteLink("DeleteActor", new { result.Data.Id }, "actor"),
			],
		};

		return Ok(response);
	}

	/// <summary>
	/// Create a new actor.
	/// </summary>
	/// <param name="createActorDto">Actor to create.</param>
	/// <returns></returns>
	[HttpPost(Name = "CreateActor")]
	[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LinkResponse<CreateActorCommandResponse>))]
	[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse))]
	public async Task<IActionResult> Post([FromForm] CreateActorDto createActorDto)
	{
		var result = await mediator.Send(new CreateActorCommand(
			createActorDto.Name,
			createActorDto.Birthdate,
			createActorDto.Photo?.OpenReadStream(),
			createActorDto.Photo?.ContentType)
		);

		var response = new LinkResponse<CreateActorCommandResponse>(
			result.Data, result.Message, result.Success, result.ValidationErrors)
		{
			Links =
			[
				linkService.AddSelfLink("GetActor", new { result.Data.Id }),
				linkService.AddUpdateLink("UpdateActor", "actor"),
				linkService.AddPartialUpdateLink("PartialUpdateActor", new { result.Data.Id }, "actor"),
				linkService.AddDeleteLink("DeleteActor", new { result.Data.Id }, "actor"),
			],
		};

		return CreatedAtRoute("GetActor", new { id = result.Data.Id }, response);
	}

	/// <summary>
	/// Update an existing actor.
	/// </summary>
	/// <param name="updateActorDto">Actor to update.</param>
	/// <returns></returns>
	[HttpPut(Name = "UpdateActor")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponse))]
	public async Task<IActionResult> Put([FromForm] UpdateActorDto updateActorDto)
	{
		await mediator.Send(new UpdateActorCommand(
			updateActorDto.Id,
			updateActorDto.Name,
			updateActorDto.Birthdate,
			updateActorDto.Photo?.OpenReadStream(),
			updateActorDto.Photo?.ContentType)
		);

		return NoContent();
	}

	/// <summary>
	/// Update an existing actor.
	/// </summary>
	/// /// <param name="id">Id of the actor to update.</param>
	/// <param name="jsonPatchDocument">Actor to update.</param>
	/// <returns></returns>
	[HttpPatch("{id:int}", Name = "PartialUpdateActor")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponse))]
	public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] JsonPatchDocument<PartialUpdateActorCommand> jsonPatchDocument)
	{
		var command = new PartialUpdateActorCommand();
		jsonPatchDocument.ApplyTo(command);
		command.Id = id;
		await mediator.Send(command);

		return NoContent();
	}

	/// <summary>
	/// Delete an actor by Id.
	/// </summary>
	/// <param name="id">Id of the actor to delete.</param>
	/// <returns></returns>
	[HttpDelete("{id:int}", Name = "DeleteActor")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseResponse))]
	public async Task<IActionResult> Delete([FromRoute] int id)
	{
		await mediator.Send(new DeleteActorCommand(id));

		return NoContent();
	}
}
