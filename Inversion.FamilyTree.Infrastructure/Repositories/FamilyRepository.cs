﻿using Inversion.FamilyTree.Application.AbstractRepositories;
using Inversion.FamilyTree.Domain.Entities;
using Inversion.FamilyTree.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Inversion.FamilyTree.Infrastructure.Repositories;
internal class FamilyRepository(FamilyDbContext dbContext) : IFamilyRepository
{
	public async Task<bool> CheckIfPersonHasChildrenAsync(int id) => id > 0 && await dbContext.People.AnyAsync(x => ( x.Father != null && x.Father.Id == id ) || ( x.Father != null && x.Father.Id == id ));
	public Task<Person?> GetPersonByIdentityNumberAsync(string identityNumber) => dbContext.People.SingleOrDefaultAsync(x => x.IdentityNumber == identityNumber);
	public Task<List<Person>> GetPersonFamilyAsync(Person person) => throw new NotImplementedException( );
}