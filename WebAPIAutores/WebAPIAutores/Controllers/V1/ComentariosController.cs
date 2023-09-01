using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;
using WebAPIAutores.Utilidades;

namespace WebAPIAutores.Controllers.V1
{
	[Route("api/v1/Libros/{libroId:int}/[controller]")]
	[ApiController]
	public class ComentariosController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly UserManager<IdentityUser> _userManager;

		public ComentariosController(
			ApplicationDbContext context,
			IMapper mapper,
			UserManager<IdentityUser> userManager)
		{
			_context = context;
			_mapper = mapper;
			_userManager = userManager;
		}

		[HttpGet(Name = "ObtenerComentariosLibro")]
		public async Task<ActionResult<List<ComentarioDto>>> Get(int libroId,
			[FromQuery] PaginacionDto paginacionDto)
		{
			var existeLibro = await _context.Libros.AnyAsync(libroBd => libroBd.Id == libroId);

			if (!existeLibro)
				return NotFound();

			var queryable = _context.Comentarios
				.Where(comentarioBD => comentarioBD.LibroId == libroId)
				.AsQueryable();
			await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);

			var comentarios = await queryable
				.OrderBy(comentario => comentario.Id)
				.Paginar(paginacionDto)
				.ToListAsync();

			return _mapper.Map<List<ComentarioDto>>(comentarios);
		}

		[HttpGet("{id:int}", Name = "ObtenerComentario")]
		public async Task<ActionResult<ComentarioDto>> GetById(int id)
		{
			var comentario = await _context.Comentarios
				.FirstOrDefaultAsync(comentarioBd => comentarioBd.Id == id);

			if (comentario == null)
				return NotFound();

			return _mapper.Map<ComentarioDto>(comentario);
		}

		[HttpPost(Name = "CrearComentario")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<ComentarioCreacionDto>> Post(int libroId, ComentarioCreacionDto comentarioCreacionDto)
		{
			var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
			var email = emailClaim.Value;
			var usuario = await _userManager.FindByEmailAsync(email);
			var usuarioId = usuario.Id;

			var existeLibro = await _context.Libros.AnyAsync(libroBd => libroBd.Id == libroId);

			if (!existeLibro)
				return NotFound();

			var comentario = _mapper.Map<Comentario>(comentarioCreacionDto);
			comentario.LibroId = libroId;
			comentario.UsuarioId = usuarioId;

			_context.Add(comentario);
			await _context.SaveChangesAsync();

			var comentarioDto = _mapper.Map<ComentarioDto>(comentario);

			return CreatedAtRoute("ObtenerComentario", new { id = comentarioDto.Id, libroId }, comentarioDto);
		}

		[HttpPut("{id:int}", Name = "ActualizarComentario")]
		public async Task<ActionResult> Put(int libroId, int id, ComentarioCreacionDto comentarioCreacionDto)
		{
			var existeLibro = await _context.Libros.AnyAsync(libroBd => libroBd.Id == libroId);

			if (!existeLibro)
				return NotFound();

			var existeComentario = await _context.Comentarios.AnyAsync(comentarioBd => comentarioBd.Id == id);

			if (!existeComentario)
				return NotFound();

			var comentario = _mapper.Map<Comentario>(comentarioCreacionDto);
			comentario.Id = id;
			comentario.LibroId = libroId;

			_context.Update(comentario);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
