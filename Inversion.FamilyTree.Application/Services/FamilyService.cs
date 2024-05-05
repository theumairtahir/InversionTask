using AutoMapper;
using Inversion.FamilyTree.Application.AbstractRepositories;
using Inversion.FamilyTree.Application.DataObjects;
using Inversion.FamilyTree.Application.Exceptions;
using Inversion.FamilyTree.Domain.Entities;

namespace Inversion.FamilyTree.Application.Services;

public interface IFamilyService
{
	Task<FamilyDto> SearchFamilyTree(FamilySearchDto familySearchDto);
	Task<PersonDto> SearchRootAncestor(FamilySearchDto familySearchDto);
}

internal class FamilyService(IFamilyRepository familyRepository, IMapper mapper) : IFamilyService
{
	public async Task<PersonDto> SearchRootAncestor(FamilySearchDto familySearchDto)
	{
		var person = await familyRepository.GetPersonByIdentityNumberAsync(familySearchDto.IdentityNumber) ?? throw new PersonNotFoundException( );
		var rootAncestor = GetRootAncestor(person);
		return mapper.Map<PersonDto>(rootAncestor);
	}

	public async Task<FamilyDto> SearchFamilyTree(FamilySearchDto familySearchDto)
	{
		var person = await familyRepository.GetPersonByIdentityNumberAsync(familySearchDto.IdentityNumber) ?? throw new PersonNotFoundException( );
		var family = await familyRepository.GetPersonFamilyAsync(person);
		return await GetFamilyTree(person, family);
	}

	private async Task<FamilyDto> GetFamilyTree(Person person, List<Person> family, int level = 1)
	{
		var familyDto = mapper.Map<FamilyDto>(person);
		var children = family.Where(p => p.Father?.Id == person.Id || p.Mother?.Id == person.Id).ToList( );
		if (children.Count > 0)
			level++;

		foreach (var child in children)
			familyDto.Children.Add(await GetFamilyTree(child, family, level));

		if (level is 10)
			familyDto.HasMoreChildren = await familyRepository.CheckIfPersonHasChildrenAsync(familyDto.Id);

		return familyDto;
	}

	private static Person GetRootAncestor(Person person)
	{
		if (person.Father is null && person.Mother is null)
			return person;
		return GetRootAncestor(person.Father ?? person.Mother!);
	}
}
