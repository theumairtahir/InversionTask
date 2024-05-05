using Inversion.FamilyTree.Application.AbstractRepositories;
using Inversion.FamilyTree.Application.DataObjects;
using Inversion.FamilyTree.Application.Exceptions;
using Inversion.FamilyTree.Application.Resolvers;
using Inversion.FamilyTree.Domain.Entities;

namespace Inversion.FamilyTree.Application.Services;

public interface IFamilyService
{
	Task<FamilyDto> SearchFamilyTree(FamilySearchDto familySearchDto);
	Task<List<FamilyDto>> SearchFamilyTree(int personId);
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
		var family = await familyRepository.GetPersonFamilyAsync(person, maxLevels: 10);
		return BuildFamilyTree(resolver.ResolveFamily(person), family);
	}

	public async Task<List<FamilyDto>> SearchFamilyTree(int personId)
	{
		var person = await familyRepository.GetPersonAsync(personId) ?? throw new PersonNotFoundException( );
		var family = await familyRepository.GetPersonFamilyAsync(person, maxLevels: 10);
		return BuildFamilyTree(resolver.ResolveFamily(person), family).Children;
	}

	public FamilyDto BuildFamilyTree(FamilyDto root, List<FamilyPersonDto> family)
	{
		var fatherGroup = family.Where(pair => pair.FatherId is not null).GroupBy(pair => pair.FatherId!.Value).ToDictionary(group => group.Key, group => group.Select(person => person).ToList( ));
		var motherGroup = family.Where(pair => pair.MotherId is not null).GroupBy(pair => pair.MotherId!.Value).ToDictionary(group => group.Key, group => group.Select(person => person).ToList( ));
		return BuildFamilyTreeUsingGrouping(root, fatherGroup, motherGroup);
	}

	private FamilyDto BuildFamilyTreeUsingGrouping(FamilyDto rootDto, Dictionary<int, List<FamilyPersonDto>> fatherGroup, Dictionary<int, List<FamilyPersonDto>> motherGroup)
	{
		var stack = new Stack<FamilyDto>( );
		stack.Push(rootDto);

		var processed = new HashSet<int>( );

		while (stack.Count > 0)
		{
			var current = stack.Pop( );

			if (!processed.Add(current.Id) || current.HasMoreChildren)
				continue;

			if (fatherGroup.TryGetValue(current.Id, out List<FamilyPersonDto>? value))
			{
				var childrenDtos = new List<FamilyDto>( );
				var children = value ?? [ ];

				foreach (var child in children)
				{
					var childDto = resolver.ResolveFamilyPerson(child);
					childrenDtos.Add(childDto);
					stack.Push(childDto);
				}

				current.Children.AddRange(childrenDtos);
			}
			else if (motherGroup.TryGetValue(current.Id, out List<FamilyPersonDto>? value2))
			{
				var childrenDtos = new List<FamilyDto>( );
				var children = value2 ?? [ ];

				foreach (var child in children)
				{
					var childDto = resolver.ResolveFamilyPerson(child);
					childrenDtos.Add(childDto);
					stack.Push(childDto);
				}

				current.Children.AddRange(childrenDtos);
			}
		}

		return rootDto;
	}
	private async Task<Person> GetRootAncestorAsync(Person person)
	{
		var ancestor = await familyRepository.GetPersonAsync(person.FatherId ?? person.MotherId);
		if (ancestor is null)
			return person;
		return await GetRootAncestorAsync(ancestor);
	}
}
