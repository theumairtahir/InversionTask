namespace Inversion.FamilyTree.Domain.Entities;

public class Person
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public required string SurName { get; set; }
	public DateOnly BirthDate { get; set; }
	public required string IdentityNumber { get; set; }
	public int? FatherId { get; set; }
	public int? MotherId { get; set; }
}
