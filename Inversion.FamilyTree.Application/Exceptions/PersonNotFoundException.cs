namespace Inversion.FamilyTree.Application.Exceptions;
internal class PersonNotFoundException : Exception
{
	public PersonNotFoundException( ) : base("Person not found!") { }
}
