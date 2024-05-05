using Inversion.FamilyTree.Application.AbstractRepositories;
using Inversion.FamilyTree.Application.DataObjects;
using Inversion.FamilyTree.Application.Exceptions;
using Inversion.FamilyTree.Application.Resolvers;
using Inversion.FamilyTree.Application.Services;
using Inversion.FamilyTree.Domain.Entities;
using Moq;
using NUnit.Framework.Internal;

namespace Inversion.FamilyTree.Tests;

[TestFixture]
public class FamilyServiceTests
{
	private Mock<IFamilyRepository> familyRepositoryMock;
	private Mock<IPersonResolver> personResolverMock;
	private IFamilyService familyService;

	[SetUp]
	public void SetUp( )
	{
		familyRepositoryMock = new Mock<IFamilyRepository>( );
		personResolverMock = new Mock<IPersonResolver>( );
		familyService = new FamilyService(familyRepositoryMock.Object, personResolverMock.Object);
	}


	[Test]
	public async Task SearchRootAncestor_ValidIdentityNumber_ReturnsRootAncestorDto( )
	{
		// Arrange
		var familySearchDto = new FamilySearchDto("12345678901");
		var person = new Person
		{
			Id = 1,
			Name = "John",
			SurName = "Doe",
			BirthDate = new DateOnly(1990, 1, 1),
			IdentityNumber = "12345678901"
		};
		familyRepositoryMock.Setup(x => x.GetPersonByIdentityNumberAsync(familySearchDto.IdentityNumber)).ReturnsAsync(person);
		personResolverMock.Setup(x => x.Resolve(It.IsAny<Person>( ))).Returns<Person>(x => new PersonDto(x.Id, x.Name, x.SurName, x.BirthDate, x.IdentityNumber, x.FatherId, x.MotherId));

		// Act
		var result = await familyService.SearchRootAncestor(familySearchDto);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(1));
		Assert.That(result.Name, Is.EqualTo("John"));
		Assert.That(result.SurName, Is.EqualTo("Doe"));
		Assert.That(result.BirthDate, Is.EqualTo(new DateOnly(1990, 1, 1)));
		Assert.That(result.IdentityNumber, Is.EqualTo("12345678901"));
	}

	[Test]
	public async Task SearchFamilyTree_ValidIdentityNumberWithSimpleFamilyTree_ReturnsFamilyTreeDto( )
	{
		// Arrange
		var familySearchDto = new FamilySearchDto("12345678901");
		var person = new Person
		{
			Id = 1,
			Name = "John",
			SurName = "Doe",
			BirthDate = new DateOnly(1990, 1, 1),
			IdentityNumber = "12345678901"
		};
		var family = new List<Person>
		{
			new( ) {
				Id = 2,
				Name = "Jane",
				SurName = "Doe",
				BirthDate = new DateOnly(1990, 1, 1),
				IdentityNumber = "12345678902",
				FatherId = person.Id
			}
		};
		familyRepositoryMock.Setup(x => x.GetPersonByIdentityNumberAsync(familySearchDto.IdentityNumber)).ReturnsAsync(person);
		familyRepositoryMock.Setup(x => x.GetPersonFamilyAsync(person)).ReturnsAsync(family);
		personResolverMock.Setup(x => x.ResolveFamily(It.IsAny<Person>( ))).Returns<Person>(x => new FamilyDto(x.Id, x.Name, x.SurName, x.BirthDate, x.IdentityNumber));

		// Act
		var result = await familyService.SearchFamilyTree(familySearchDto);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(1));
		Assert.That(result.Name, Is.EqualTo("John"));
		Assert.That(result.SurName, Is.EqualTo("Doe"));
		Assert.That(result.BirthDate, Is.EqualTo(new DateOnly(1990, 1, 1)));
		Assert.That(result.IdentityNumber, Is.EqualTo("12345678901"));
		Assert.That(result.Children.Count, Is.EqualTo(1));
		Assert.That(result.Children[0].Id, Is.EqualTo(2));
		Assert.That(result.Children[0].Name, Is.EqualTo("Jane"));
		Assert.That(result.Children[0].SurName, Is.EqualTo("Doe"));
		Assert.That(result.Children[0].BirthDate, Is.EqualTo(new DateOnly(1990, 1, 1)));
		Assert.That(result.Children[0].IdentityNumber, Is.EqualTo("12345678902"));
	}

	[Test]
	public async Task SearchFamilyTree_ValidIdentityNumberWithFamilyTreeMoreThan10Levels_ReturnsFamilyTreeDto( )
	{
		// Arrange
		var familySearchDto = new FamilySearchDto("12345678901");
		var person = new Person
		{
			Id = 1,
			Name = "John",
			SurName = "Doe",
			BirthDate = new DateOnly(1990, 1, 1),
			IdentityNumber = "12345678901"
		};
		var family = new List<Person>
		{
			new( )
			{
				Id = 2,
				Name = "Jane",
				SurName = "Doe",
				BirthDate = new DateOnly(1990, 1, 1),
				IdentityNumber = "12345678902",
				FatherId = person.Id
			}
		};
		for (int i = 0; i < 10; i++)
		{
			family.Add(new Person
			{
				Id = i + 3,
				Name = "Child",
				SurName = "Doe",
				BirthDate = new DateOnly(1990, 1, 1),
				IdentityNumber = $"1234567890{i + 3}",
				FatherId = family[i].Id
			});
		}
		familyRepositoryMock.Setup(x => x.GetPersonByIdentityNumberAsync(familySearchDto.IdentityNumber)).ReturnsAsync(person);
		familyRepositoryMock.Setup(x => x.GetPersonFamilyAsync(person)).ReturnsAsync(family);
		familyRepositoryMock.Setup(x => x.CheckIfPersonHasChildrenAsync(It.IsAny<int>( ))).ReturnsAsync(true);
		personResolverMock.Setup(x => x.ResolveFamily(It.IsAny<Person>( ))).Returns<Person>(x => new FamilyDto(x.Id, x.Name, x.SurName, x.BirthDate, x.IdentityNumber));

		// Act
		var result = await familyService.SearchFamilyTree(familySearchDto);

		// Assert
		Assert.That(result, Is.Not.Null);
		Assert.That(result.Id, Is.EqualTo(1));
		Assert.That(result.Name, Is.EqualTo("John"));
		Assert.That(result.SurName, Is.EqualTo("Doe"));
		Assert.That(result.BirthDate, Is.EqualTo(new DateOnly(1990, 1, 1)));
		//validate that the last level's 'HasMoreChildren' Flag is 'true'

	}

	[Test]
	public void SearchRootAncestor_InvalidIdentityNumber_ThrowsPersonNotFoundException( )
	{
		// Arrange
		var familySearchDto = new FamilySearchDto("12345678901");
		familyRepositoryMock.Setup(x => x.GetPersonByIdentityNumberAsync(familySearchDto.IdentityNumber)).ReturnsAsync((Person)null);

		// Act & Assert
		Assert.ThrowsAsync<PersonNotFoundException>(async ( ) => await familyService.SearchRootAncestor(familySearchDto));
	}

	[Test]
	public void SearchFamilyTree_InvalidIdentityNumber_ThrowsPersonNotFoundException( )
	{
		// Arrange
		var familySearchDto = new FamilySearchDto("12345678901");
		familyRepositoryMock.Setup(x => x.GetPersonByIdentityNumberAsync(familySearchDto.IdentityNumber)).ReturnsAsync((Person)null);

		// Act & Assert
		Assert.ThrowsAsync<PersonNotFoundException>(async ( ) => await familyService.SearchFamilyTree(familySearchDto));
	}
}
