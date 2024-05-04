using Inversion.FamilyTree.Application.DataObjects;
using Inversion.FamilyTree.Domain.Entities;

namespace Inversion.FamilyTree.Application.AbstractRepositories;
public interface IFamilyRepository
{
	Task<FamilyDto> GetFamilyByIdentityNumberAsync(string identityNumber);
	Task<Person> GetPersonByIdentityNumberAsync(string identityNumber);
}
