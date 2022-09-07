using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores.DTOs
{
    public class EditarAdminDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
