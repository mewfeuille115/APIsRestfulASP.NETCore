using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entidades;
using WebAPIAutores.Filtros;
using WebAPIAutores.Servicios;

namespace WebAPIAutores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IServicio _servicio;
        private readonly ServicioTransient _servicioTransient;
        private readonly ServicioScoped _servicioScoped;
        private readonly ServicioSingleton _servicioSingleton;
        private readonly ILogger<AutoresController> _logger;

        public AutoresController(
            ApplicationDbContext context,
            IServicio servicio,
            ServicioTransient servicioTransient,
            ServicioScoped servicioScoped,
            ServicioSingleton servicioSingleton,
            ILogger<AutoresController> logger)
        {
            _context = context;
            _servicio = servicio;
            _servicioTransient = servicioTransient;
            _servicioScoped = servicioScoped;
            _servicioSingleton = servicioSingleton;
            _logger = logger;
        }

        [HttpGet("GUID")]
        //[ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public ActionResult ObtenerGuids()
        {
            return Ok(new
            {
                AutoresController_Transient = _servicioTransient.Guid,
                ServicioA_Transient = _servicio.ObtenerTransient(),

                AutoresController_Scoped = _servicioScoped.Guid,
                ServicioA_Scoped = _servicio.ObtenerScoped(),
                
                AutoresController_Singleton = _servicioSingleton.Guid,
                ServicioA_Singleton = _servicio.ObtenerSingleton()
            });
        }

        [HttpGet] // api/autores
        [HttpGet("listado")] // api/autores/listado
        [HttpGet("/listado")] // listado
        //[ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        //[Authorize]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            // Esta excepción se agrega al logger por medio de FiltroDeExcepcion configurado en Startup.cs
            //throw new NotImplementedException();

            _logger.LogInformation("Estamos obteniendo los autores.");
            _logger.LogWarning("Este es un mensaje de prueba.");
            _servicio.RealizarTarea();
            return await _context.Autores.Include(x => x.Libros).ToListAsync();
        }

        [HttpGet("primero")] // api/autores/primero?nombre=sergio&apellido=perez
        public async Task<ActionResult<Autor>> PrimerAutor([FromHeader] int miValor, [FromQuery] string nombre)
        {
            return await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync();
        }
        
        // Como consulta en memoria (No es operacion I/O) no es necesario marcar el método como Asincrono.
        [HttpGet("primero2")] // api/autores/primero2
        public ActionResult<Autor> PrimerAutor2()
        {
            return new Autor() { Nombre = "Inventado" };
        }

        //[HttpGet("{id:int}/{param2?}")]
        [HttpGet("{id:int}/{param2=persona}")]
        public async Task<ActionResult<Autor>> Get(int id, string param2)
        {
            var autor = await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
                return NotFound();

            return autor;
        }


        [HttpGet("{nombre}")] // No existe restricción para String, da error.
        public async Task<ActionResult<Autor>> Get(string nombre)
        {
            var autor = await _context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (autor == null)
                return NotFound();

            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            var existeAutorConElMismoNombre = await _context.Autores.AnyAsync(x => x.Nombre == autor.Nombre);

            if (existeAutorConElMismoNombre)
                return BadRequest($"Ya existe un autor con el nombre {autor.Nombre}");

            _context.Add(autor);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id:int}")] // api/Autores/1
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
                return BadRequest("El id del autor no coincide con el id de la URL");
            
            var existe = await _context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();
            
            _context.Update(autor);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")] // api/Autores/2
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();
            
            _context.Remove(new Autor() { Id = id});
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
