using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers.V1
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class LibrosController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public LibrosController(
			ApplicationDbContext context,
			IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet("{id:int}", Name = "ObtenerLibro")]
		public async Task<ActionResult<LibroDtoConAutores>> Get(int id)
		{
			var libro = await _context.Libros
				.Include(libroBd => libroBd.AutoresLibros)
				.ThenInclude(autoreLibroBd => autoreLibroBd.Autor)
				.FirstOrDefaultAsync(x => x.Id == id);

			if (libro == null)
				return NotFound();

			libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();

			return _mapper.Map<LibroDtoConAutores>(libro);
		}

		[HttpPost(Name = "CrearLibro")]
		public async Task<ActionResult> Post(LibroCreacionDto libroCreacionDto)
		{
			if (libroCreacionDto.AutoresIds == null)
				return BadRequest("No se puede crear un libro sin autores.");

			var autoresIds = await _context.Autores
				.Where(autorBd => libroCreacionDto.AutoresIds.Contains(autorBd.Id))
				.Select(x => x.Id)
				.ToListAsync();

			if (libroCreacionDto.AutoresIds.Count != autoresIds.Count)
				return BadRequest("No existe uno de los autores enviados.");

			var libro = _mapper.Map<Libro>(libroCreacionDto);

			AsignarOrdenAutores(libro);

			_context.Add(libro);
			await _context.SaveChangesAsync();

			var libroDto = _mapper.Map<LibroDto>(libro);

			return CreatedAtRoute("ObtenerLibro", new { id = libroDto.Id }, libroDto);
		}

		[HttpPut("{id:int}", Name = "ActualizarLibro")]
		public async Task<ActionResult> Put(int id, LibroCreacionDto libroCreacionDto)
		{
			var libroBd = await _context.Libros
				.Include(autoresBd => autoresBd.AutoresLibros)
				.FirstOrDefaultAsync(x => x.Id == id);

			if (libroBd == null)
				return NotFound();

			libroBd = _mapper.Map(libroCreacionDto, libroBd);
			AsignarOrdenAutores(libroBd);

			await _context.SaveChangesAsync();

			return NoContent();
		}

		private void AsignarOrdenAutores(Libro libro)
		{
			if (libro.AutoresLibros != null)
			{
				for (int i = 0; i < libro.AutoresLibros.Count; i++)
				{
					libro.AutoresLibros[i].Orden = i;
				}
			}
		}

		// Ejemplo:
		// Campo id: 7
		//
		//[
		//  {
		//    "path": "/titulo",
		//    "op": "replace",
		//    "value": "Titulo desde patch"
		//  }
		//]
		[HttpPatch(Name = "PatchLibro")]
		public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDto> patchDocument)
		{
			if (patchDocument == null)
				return BadRequest();

			var libroBd = await _context.Libros.FirstOrDefaultAsync(x => x.Id == id);

			if (libroBd == null)
				return NotFound();

			var libroDto = _mapper.Map<LibroPatchDto>(libroBd);

			patchDocument.ApplyTo(libroDto, ModelState);

			var esValido = TryValidateModel(libroDto);

			if (!esValido)
				return BadRequest(ModelState);

			_mapper.Map(libroDto, libroBd);

			await _context.SaveChangesAsync();

			return NoContent();
		}

		[HttpDelete("{id:int}", Name = "BorrarLibro")]
		public async Task<ActionResult> Delete(int id)
		{
			var existe = await _context.Libros.AnyAsync(x => x.Id == id);

			if (!existe)
				return NotFound();

			_context.Remove(new Libro() { Id = id });
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
