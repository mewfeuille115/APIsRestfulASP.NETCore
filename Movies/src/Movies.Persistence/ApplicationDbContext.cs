using Microsoft.EntityFrameworkCore;
using Movies.Domain.Entities;

namespace Movies.Persistence;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<Genre> Genres => Set<Genre>();
}