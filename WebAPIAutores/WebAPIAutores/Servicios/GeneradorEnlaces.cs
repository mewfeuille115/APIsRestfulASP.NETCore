using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WebAPIAutores.DTOs;

namespace WebAPIAutores.Servicios
{
	public class GeneradorEnlaces
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IActionContextAccessor _actionContextAccessor;

		public GeneradorEnlaces(IAuthorizationService authorizationService,
			IHttpContextAccessor httpContextAccessor,
			IActionContextAccessor actionContextAccessor)
		{
			_authorizationService = authorizationService;
			_httpContextAccessor = httpContextAccessor;
			_actionContextAccessor = actionContextAccessor;
		}

		private IUrlHelper ConstruirUrlHelper()
		{
			var factoria = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
			return factoria.GetUrlHelper(_actionContextAccessor.ActionContext);
		}

		private async Task<bool> EsAdmin()
		{
			var httpContext = _httpContextAccessor.HttpContext;
			var resultado = await _authorizationService.AuthorizeAsync(httpContext.User, "EsAdmin");
			return resultado.Succeeded;
		}

		public async Task GenerarEnlaces(AutorDto autorDto)
		{
			var esAdmin = await EsAdmin();
			var Url = ConstruirUrlHelper();

			autorDto.Enlaces.Add(new DatoHateoas(
				enlace: Url.Link("ObtenerAutor", new { id = autorDto.Id }),
				descripcion: "self",
				metodo: "GET"));

			if (esAdmin)
			{
				autorDto.Enlaces.Add(new DatoHateoas(
					enlace: Url.Link("ActualizarAutor", new { id = autorDto.Id }),
					descripcion: "autor-actualizar",
					metodo: "PUT"));
				autorDto.Enlaces.Add(new DatoHateoas(
					enlace: Url.Link("BorrarAutor", new { id = autorDto.Id }),
					descripcion: "autor-borrar",
					metodo: "DELETE"));
			}
		}

		//TODO: Posible metodo para meter los enlaces que faltan a nivel de listado (Se tiene que reemplazar la respuesta del metodo del controller)
		//public async Task GenerarEnlacesListado(List<AutorDto> autoresDtos)
		//{
		//	var esAdmin = await EsAdmin();
		//	var Url = ConstruirUrlHelper();

		//	var resultado = new ColeccionDeRecursos<AutorDto> { Valores = autoresDtos };
		//	resultado.Enlaces.Add(new DatoHateoas(
		//	enlace: Url.Link("ObtenerAutores", new { }),
		//	descripcion: "self",
		//	metodo: "GET"));

		//	if (esAdmin)
		//	{
		//		resultado.Enlaces.Add(new DatoHateoas(
		//			enlace: Url.Link("CrearAutor", new { }),
		//			descripcion: "autor-crear",
		//			metodo: "POST"));
		//	}
		//}
	}
}
