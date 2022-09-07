using Microsoft.AspNetCore.Identity;

namespace WebAPIAutores.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public int LibroId { get; set; }
        public string UsuarioId { get; set; }

        public Libro Libro { get; set; }
        public IdentityUser Usuario { get; set; }
    }
}
