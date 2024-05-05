using Inversion.FamilyTree.Application.AbstractRepositories;
using Inversion.FamilyTree.Application.DataObjects;
using Inversion.FamilyTree.Application.Exceptions;
using Inversion.FamilyTree.Application.Resolvers;
using Inversion.FamilyTree.Domain.Entities;

namespace Inversion.FamilyTree.Application.Services;

public interface IFamilyService
{
	Task<FamilyDto> SearchFamilyTree(FamilySearchDto familySearchDto);
	Task<PersonDto> SearchRootAncestor(FamilySearchDto familySearchDto);
}

internal class FamilyService(IFamilyRepository familyRepository, IPersonResolver resolver) : IFamilyService
{
	public async Task<PersonDto> SearchRootAncestor(FamilySearchDto familySearchDto)
	{
		var person = await familyRepository.GetPersonByIdentityNumberAsync(familySearchDto.IdentityNumber) ?? throw new PersonNotFoundException( );
		var rootAncestor = await GetRootAncestorAsync(person);
		return resolver.Resolve(rootAncestor);
	}

	public async Task<FamilyDto> SearchFamilyTree(FamilySearchDto familySearchDto)
	{
		var person = await familyRepository.GetPersonByIdentityNumberAsync(familySearchDto.IdentityNumber) ?? throw new PersonNotFoundException( );
		var family = await familyRepository.GetPersonFamilyAsync(person, 3); //fetching 3 levels deep due to performance reasons
		return await GetFamilyTree(resolver.ResolveFamily(person), family);
	}

	private async Task<FamilyDto> GetFamilyTree(FamilyDto familyDto, List<FamilyPersonDto> family)
	{
		var children = family.Where(p => p.FatherId == familyDto.Id || p.MotherId == familyDto.Id).ToList( );
		foreach (var child in children)
			familyDto.Children.Add(await GetFamilyTree(resolver.ResolveFamilyPerson(child), family));
		return familyDto;
	}

	private async Task<Person> GetRootAncestorAsync(Person person)
	{
		var ancestor = await familyRepository.GetPersonAsync(person.FatherId ?? person.MotherId);
		if (ancestor is null)
			return person;
		return await GetRootAncestorAsync(ancestor);
	}
}
