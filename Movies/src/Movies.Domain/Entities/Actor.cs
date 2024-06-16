using Movies.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Movies.Domain.Entities;

public class Actor : AuditableEntity
{
	public int Id { get; set; }

	[Required]
	[StringLength(120)]
	public string Name { get; set; } = null!;
	public DateTime Birthdate { get; set; }
    public string? PhotoUrl { get; set; }
}
