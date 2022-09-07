using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.DTOs;
using WebAPIAutores.Entidades;
using WebAPIAutores.Filtros;

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AutoresController(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet] // api/autores
        [AllowAnonymous]
        public async Task<ActionResult<List<AutorDto>>> Get()
        {
            //var autores = await _context.Autores.Include(x => x.Libros).ToListAsync();
            var autores = await _context.Autores.ToListAsync();

            if (autores.Count.Equals(0))
                return NotFound();

            return _mapper.Map<List<AutorDto>>(autores);
        }

        [HttpGet("{id:int}", Name = "ObtenerAutor" )]
        public async Task<ActionResult<AutorDtoConLibros>> Get(int id)
        {
            var autor = await _context.Autores
                .Include(autorBd => autorBd.AutoresLibros)
                .ThenInclude(autorLibroBd => autorLibroBd.Libro)
                .FirstOrDefaultAsync(autorBd => autorBd.Id == id);

            if (autor == null)
                return NotFound();

            return _mapper.Map<AutorDtoConLibros>(autor);
        }

        [HttpGet("{nombre}")] // No existe restricción para String, da error.
        public async Task<ActionResult<List<AutorDto>>> Get([FromRoute] string nombre)
        {
            var autores = await _context.Autores.Where(autorBd => autorBd.Nombre.Contains(nombre)).ToListAsync();

            if (autores.Count.Equals(0))
                return NotFound();

            return _mapper.Map<List<AutorDto>>(autores);
        }

        [HttpPost]
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

        [HttpPut("{id:int}")] // api/Autores/1
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

        [HttpDelete("{id:int}")] // api/Autores/2
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();
            
            _context.Remove(new Autor() { Id = id});
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
