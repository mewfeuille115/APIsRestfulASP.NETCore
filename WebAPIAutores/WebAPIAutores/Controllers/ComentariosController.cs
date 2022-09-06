using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;

namespace WebAPIAutores.Controllers
{
    [Route("api/Libros/{libroId:int}/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ComentariosController(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDto>>> Get(int libroId)
        {
            var existeLibro = await _context.Libros.AnyAsync(libroBd => libroBd.Id == libroId);

            if (!existeLibro)
                return NotFound();

            var comentarios = await _context.Comentarios
                .Where(comentarioBD => comentarioBD.LibroId == libroId)
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

        [HttpPost]
        public async Task<ActionResult<ComentarioCreacionDto>> Post(int libroId, ComentarioCreacionDto comentarioCreacionDto)
        {
            var existeLibro = await _context.Libros.AnyAsync(libroBd => libroBd.Id == libroId);

            if (!existeLibro)
                return NotFound();

            var comentario = _mapper.Map<Comentario>(comentarioCreacionDto);
            comentario.LibroId = libroId;
            
            _context.Add(comentario);
            await _context.SaveChangesAsync();

            var comentarioDto = _mapper.Map<ComentarioDto>(comentario);

            return CreatedAtRoute("ObtenerComentario", new { id = comentarioDto.Id, libroId = libroId }, comentarioDto);
        }

        [HttpPut("{id:int}")]
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
