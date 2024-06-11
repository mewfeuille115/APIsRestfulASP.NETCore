using Movies.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Movies.Domain.Entities;

public class Genre : AuditableEntity
{
	public int Id { get; set; }

	[Required]
	[StringLength(40)]
	public string Name { get; set; } = null!;
}
