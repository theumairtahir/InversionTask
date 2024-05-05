using Inversion.FamilyTree.Application.DataObjects;
using Inversion.FamilyTree.Application.Resolvers;
using Inversion.FamilyTree.Application.Services;
using Inversion.FamilyTree.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Inversion.FamilyTree.Application;
public static class Setup
{
	public static void AddApplication(this IServiceCollection services)
	{
		services.AddAutoMapper(config =>
		{
			config.CreateMap<Person, PersonDto>( );
			config.CreateMap<Person, FamilyDto>( ).ForCtorParam(nameof(FamilyDto.HasMoreChildren), o => o.MapFrom(p => false));
			config.CreateMap<FamilyPersonDto, FamilyDto>( );
		});
		services.AddScoped<IFamilyService, FamilyService>( );
		services.AddScoped<IPersonResolver, PersonResolver>( );
	}
}
