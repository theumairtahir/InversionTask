using Inversion.FamilyTree.Application;
using Inversion.FamilyTree.Application.DataObjects;
using Inversion.FamilyTree.Application.Services;
using Inversion.FamilyTree.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer( );
builder.Services.AddSwaggerGen( );
builder.Services.AddApplication( );

builder.Services.AddInfrastructure("FamilyTreeDb");

builder.Services.AddCors(x =>
{
	x.AddDefaultPolicy(p =>
	{
		p.AllowAnyOrigin( );
		p.AllowAnyMethod( );
		p.AllowAnyHeader( );
	});
});

var app = builder.Build( );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment( ))
{
	app.UseSwagger( );
	app.UseSwaggerUI( );
}

app.UseHttpsRedirection( );


app.MapGet("/get-root-ancestor/{identityNumber}", (string identityNumber, IFamilyService familyService) => familyService.SearchRootAncestor(new FamilySearchDto(identityNumber)))
.WithName("GetRootAncestor")
.WithOpenApi( );

app.MapGet("/get-family-tree/{identityNumber}", (string identityNumber, IFamilyService familyService) => familyService.SearchFamilyTree(new FamilySearchDto(identityNumber)))
.WithName("GetFamilyTree")
.WithOpenApi( );

app.Run( );
