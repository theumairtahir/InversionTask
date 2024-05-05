using Inversion.FamilyTree.Application.DataObjects;
using Inversion.FamilyTree.Domain.Entities;

namespace Inversion.FamilyTree.Application.AbstractRepositories;
public interface IFamilyRepository
{
	Task<List<FamilyPersonDto>> GetPersonFamilyAsync(Person person, int maxLevels = 10);
	Task<Person?> GetPersonByIdentityNumberAsync(string identityNumber);
	Task<bool> CheckIfPersonHasChildrenAsync(int id);
	Task<Person?> GetPersonAsync(int? id);
}
