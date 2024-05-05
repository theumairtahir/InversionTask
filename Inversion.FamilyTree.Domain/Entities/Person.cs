namespace Inversion.FamilyTree.Domain.Entities;

public class Person
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public required string SurName { get; set; }
	public DateOnly BirthDate { get; set; }
	public required string IdentityNumber { get; set; }
	public virtual Person? Father { get; set; }
	public virtual Person? Mother { get; set; }
}
