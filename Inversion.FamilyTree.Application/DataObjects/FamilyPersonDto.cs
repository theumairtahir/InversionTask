namespace Inversion.FamilyTree.Application.DataObjects;

public class FamilyPersonDto
{
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public string SurName { get; set; } = null!;
	public DateOnly BirthDate { get; set; }
	public string IdentityNumber { get; set; } = null!;
	public int? FatherId { get; set; }
	public int? MotherId { get; set; }
	public bool HasMoreChildren { get; set; }
}
