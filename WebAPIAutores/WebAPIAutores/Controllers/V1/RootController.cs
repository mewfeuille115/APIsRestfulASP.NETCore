using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores.DTOs;

namespace WebAPIAutores.Controllers.V1
{
	[Route("api/v1/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class RootController : ControllerBase
	{
		private readonly IAuthorizationService _authorizationService;

		public RootController(IAuthorizationService authorizationService)
		{
			_authorizationService = authorizationService;
		}

		[HttpGet(Name = "ObtenerRoot")]
		[AllowAnonymous]
		public async Task<ActionResult<IEnumerable<DatoHateoas>>> Get()
		{
			var datosHateoas = new List<DatoHateoas>();

			var esAdmin = await _authorizationService.AuthorizeAsync(User, "esAdmin");

			datosHateoas.Add(new DatoHateoas(enlace: Url.Link("ObtenerRoot", new { }),
				descripcion: "self", metodo: "GET"));

			datosHateoas.Add(new DatoHateoas(enlace: Url.Link("ObtenerAutores", new { }),
				descripcion: "autores", metodo: "GET"));

			if (esAdmin.Succeeded)
			{
				datosHateoas.Add(new DatoHateoas(enlace: Url.Link("CrearAutor", new { }),
					descripcion: "autor-crear", metodo: "POST"));

				datosHateoas.Add(new DatoHateoas(enlace: Url.Link("CrearLibro", new { }),
					descripcion: "libro-crear", metodo: "POST"));
			}

			return datosHateoas;
		}

	}
}
