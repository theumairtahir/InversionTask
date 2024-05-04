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

	public async Task<FamilyDto> SearchFamilyTree(FamilySearchDto familySearchDto) => await familyRepository.GetFamilyByIdentityNumberAsync(familySearchDto.IdentityNumber) ?? throw new PersonNotFoundException( );

	private static Person GetRootAncestor(Person person)
	{
		if (person.Father is null && person.Mother is null)
			return person;
		return GetRootAncestor(person.Father ?? person.Mother!);
	}
}
