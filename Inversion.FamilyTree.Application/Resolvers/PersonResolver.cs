using AutoMapper;
using Inversion.FamilyTree.Application.DataObjects;
using Inversion.FamilyTree.Domain.Entities;

namespace Inversion.FamilyTree.Application.Resolvers;

public interface IPersonResolver
{
	PersonDto Resolve(Person rootAncestor);
	FamilyDto ResolveFamily(Person person);
}

internal class PersonResolver(IMapper mapper) : IPersonResolver
{
	public PersonDto Resolve(Person rootAncestor) => mapper.Map<PersonDto>(rootAncestor);
	public FamilyDto ResolveFamily(Person person) => mapper.Map<FamilyDto>(person);
}
