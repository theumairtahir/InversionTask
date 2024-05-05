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
		var family = await familyRepository.GetPersonFamilyAsync(person);
		return await GetFamilyTree(person, family);
	}

	private async Task<FamilyDto> GetFamilyTree(Person person, List<Person> family, int level = 1)
	{
		var familyDto = resolver.ResolveFamily(person);
		var children = family.Where(p => p.FatherId == person.Id || p.MotherId == person.Id).ToList( );
		if (children.Count > 0)
			level++;

		foreach (var child in children)
			familyDto.Children.Add(await GetFamilyTree(child, family, level));

		if (level is 10)
			familyDto.HasMoreChildren = await familyRepository.CheckIfPersonHasChildrenAsync(familyDto.Id);

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
