using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIAutores.DTOs;
using WebAPIAutores.Servicios;

namespace WebAPIAutores.Utilidades
{
	public class HateoasAutorFilterAttribute : HateoasFiltroAttribute
	{
		private readonly GeneradorEnlaces _generadorEnlaces;

		public HateoasAutorFilterAttribute(GeneradorEnlaces generadorEnlaces)
		{
			_generadorEnlaces = generadorEnlaces;
		}

		public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			var debeIncluir = DebeIncluirHateoas(context);

			if (!debeIncluir)
			{
				await next();
				return;
			}

			var resultado = context.Result as ObjectResult;
			//var modelo = resultado.Value as AutorDto ?? throw new
			//	ArgumentNullException("Se esperaba una instancia de AutorDto");

			var autorDto = resultado.Value as AutorDto;
			if (autorDto == null)
			{
				var autoresDto = resultado.Value as List<AutorDto> ??
					throw new ArgumentNullException("Se esperaba una instancia de AutorDto o List<AutorDto>");

				autoresDto.ForEach(async autor => await _generadorEnlaces.GenerarEnlaces(autor));
				resultado.Value = autoresDto;
			}
			else
			{
				await _generadorEnlaces.GenerarEnlaces(autorDto);
			}

			await next();
		}
	}
}
