using Inversion.FamilyTree.Application.AbstractRepositories;
using Inversion.FamilyTree.Application.DataObjects;
using Inversion.FamilyTree.Domain.Entities;
using Inversion.FamilyTree.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Inversion.FamilyTree.Infrastructure.Repositories;
internal class FamilyRepository(FamilyDbContext dbContext) : IFamilyRepository
{
	public async Task<Person?> GetPersonAsync(int? id) => id is not null ? await dbContext.People.SingleAsync(x => x.Id == id) : null;
	public Task<Person?> GetPersonByIdentityNumberAsync(string identityNumber) => dbContext.People.SingleOrDefaultAsync(x => x.IdentityNumber == identityNumber);
	public Task<List<FamilyPersonDto>> GetPersonFamilyAsync(Person person, int maxLevels = 10) => dbContext.Database.SqlQuery<FamilyPersonDto>($"EXEC GetDescendantsByIdentityNumber @Id = {person.Id}, @MaxLevels = {maxLevels}").ToListAsync( );
}
