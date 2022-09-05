using WebAPIAutores.Validaciones;

namespace WebAPIAutores.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [PrimeraLetraMayuscula]
        public string Titulo { get; set; }
        public int AutorId { get; set; }
        public Autor Autor { get; set; }
    }
}
