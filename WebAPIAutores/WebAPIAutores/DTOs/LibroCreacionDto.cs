using System.ComponentModel.DataAnnotations;
using WebAPIAutores.Validaciones;

namespace WebAPIAutores.DTOs
{
    public class LibroCreacionDto
    {
        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 250)]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
