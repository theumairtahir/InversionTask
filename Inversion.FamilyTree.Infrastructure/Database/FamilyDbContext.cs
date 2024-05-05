using Inversion.FamilyTree.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inversion.FamilyTree.Infrastructure.Database;

internal class FamilyDbContext(DbContextOptions<FamilyDbContext> options) : DbContext(options)
{
	public DbSet<Person> People { get; set; }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Person>(entity =>
		{
			entity.HasKey(p => p.Id);
			entity.HasIndex(p => p.IdentityNumber).IsUnique( );
		});
	}
}
