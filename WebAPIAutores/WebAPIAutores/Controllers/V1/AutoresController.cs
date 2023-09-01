using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;
using WebAPIAutores.Utilidades;

namespace WebAPIAutores.Controllers.V1
{
	[Route("api/[controller]")]
	//[Route("api/v1/[controller]")]
	[CabeceraEstaPresente("x-version", "1")]
	[ApiController]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
	//[ApiConventionType(typeof(DefaultApiConventions))]
	public class AutoresController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly IAuthorizationService _authorizationService;

		public AutoresController(
			ApplicationDbContext context,
			IMapper mapper,
			IAuthorizationService authorizationService)
		{
			_context = context;
			_mapper = mapper;
			_authorizationService = authorizationService;
		}

		[HttpGet(Name = "ObtenerAutoresv1")] // api/autores
		[AllowAnonymous]
		[ServiceFilter(typeof(HateoasAutorFilterAttribute))]
		public async Task<ActionResult<List<AutorDto>>> Get([FromQuery] PaginacionDto paginacionDto)
		{
			var queryable = _context.Autores.AsQueryable();
			await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

			var autores = await queryable
				.OrderBy(autor => autor.Nombre)
				.Paginar(paginacionDto)
				.ToListAsync();

			if (autores.Count.Equals(0))
				return NotFound();

			return _mapper.Map<List<AutorDto>>(autores);
		}

		[HttpGet("{id:int}", Name = "ObtenerAutorv1")]
		[AllowAnonymous]
		[ServiceFilter(typeof(HateoasAutorFilterAttribute))]
		//[ProducesResponseType(200)]
		//[ProducesResponseType(404)]
		public async Task<ActionResult<AutorDtoConLibros>> Get(int id)
		{
			var autor = await _context.Autores
				.Include(autorBd => autorBd.AutoresLibros)
				.ThenInclude(autorLibroBd => autorLibroBd.Libro)
				.FirstOrDefaultAsync(autorBd => autorBd.Id == id);

			if (autor == null)
				return NotFound();

			var dto = _mapper.Map<AutorDtoConLibros>(autor);

			return dto;
		}

		[HttpGet("{nombre}", Name = "ObtenerAutorPorNombrev1")] // No existe restricción para String, da error.
		public async Task<ActionResult<List<AutorDto>>> GetPorNombre([FromRoute] string nombre)
		{
			var autores = await _context.Autores.Where(autorBd => autorBd.Nombre.Contains(nombre)).ToListAsync();

			if (autores.Count.Equals(0))
				return NotFound();

			return _mapper.Map<List<AutorDto>>(autores);
		}

		[HttpPost(Name = "CrearAutorv1")]
		public async Task<ActionResult> Post([FromBody] AutorCreacionDto autorCreacionDto)
		{
			var existeAutorConElMismoNombre = await _context.Autores.AnyAsync(x => x.Nombre == autorCreacionDto.Nombre);

			if (existeAutorConElMismoNombre)
				return BadRequest($"Ya existe un autor con el nombre {autorCreacionDto.Nombre}");

			var autor = _mapper.Map<Autor>(autorCreacionDto);

			_context.Add(autor);
			await _context.SaveChangesAsync();

			var autorDto = _mapper.Map<AutorDto>(autor);

			return CreatedAtRoute("ObtenerAutor", new { id = autorDto.Id }, autorDto);
		}

		[HttpPut("{id:int}", Name = "ActualizarAutorv1")] // api/Autores/1
		public async Task<ActionResult> Put(AutorCreacionDto autorCreacionDto, int id)
		{
			var existe = await _context.Autores.AnyAsync(x => x.Id == id);

			if (!existe)
				return NotFound();

			var autor = _mapper.Map<Autor>(autorCreacionDto);
			autor.Id = id;

			_context.Update(autor);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		/// <summary>
		/// Borra un autor
		/// </summary>
		/// <param name="id">Id del autor a borrar</param>
		/// <returns></returns>
		[HttpDelete("{id:int}", Name = "BorrarAutorv1")] // api/Autores/2
		public async Task<ActionResult> Delete(int id)
		{
			var existe = await _context.Autores.AnyAsync(x => x.Id == id);

			if (!existe)
				return NotFound();

			_context.Remove(new Autor() { Id = id });
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
