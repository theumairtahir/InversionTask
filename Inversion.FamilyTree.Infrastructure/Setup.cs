using Inversion.FamilyTree.Application.AbstractRepositories;
using Inversion.FamilyTree.Infrastructure.Database;
using Inversion.FamilyTree.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inversion.FamilyTree.Infrastructure;
public static class Setup
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionName)
	{
		services.AddDbContext<FamilyDbContext>((sp, options) =>
		{
			var configuration = sp.GetRequiredService<IConfiguration>( );
			options.UseSqlServer(configuration.GetConnectionString(connectionName), x =>
			{
				x.MigrationsAssembly(typeof(Setup).Assembly.FullName);
				x.CommandTimeout(300);
			});
			options.UseLazyLoadingProxies( );
		}, ServiceLifetime.Scoped, ServiceLifetime.Singleton);
		services.AddScoped<IFamilyRepository, FamilyRepository>( );
		return services;
	}
}
