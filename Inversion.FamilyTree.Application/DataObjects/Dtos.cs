namespace Inversion.FamilyTree.Application.DataObjects;

public record FamilySearchDto(string IdentityNumber);
public record PersonDto(int Id, string Name, string SurName, DateOnly BirthDate, string IdentityNumber, int? FatherId, int? MotherId);
public record FamilyDto(int Id, string Name, string SurName, DateOnly BirthDate, string IdentityNumber)
{
	public bool HasMoreChildren { get; set; }
	public List<FamilyDto> Children { get; set; } = [ ];
}
