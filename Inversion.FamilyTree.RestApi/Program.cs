using Inversion.FamilyTree.Application;
using Inversion.FamilyTree.Application.DataObjects;
using Inversion.FamilyTree.Application.Services;
using Inversion.FamilyTree.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using System.Buffers.Text;
using System.Net;
using System.Text;

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
app.UseCors( );

app.UseExceptionHandler(errorApp =>
{
	errorApp.Run(async context =>
	{
		context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
		context.Response.ContentType = "application/json";

		var error = context.Features.Get<IExceptionHandlerFeature>( );

		if (error is null)
			return;

		var ex = error.Error;

		await context.Response.WriteAsync(ex.Message, Encoding.UTF8);
	});
});

app.MapGet("/api/get-root-ancestor/{identityNumber}", (string identityNumber, IFamilyService familyService) => familyService.SearchRootAncestor(new FamilySearchDto(identityNumber)))
.WithName("GetRootAncestor")
.WithOpenApi( );

app.MapGet("/api/get-family-tree/{identityNumber}", (string identityNumber, IFamilyService familyService) => familyService.SearchFamilyTree(new FamilySearchDto(identityNumber)))
.WithName("GetFamilyTree")
.WithOpenApi( );

app.MapGet("/api/get-family-tree-node/{personId}", (int personId, IFamilyService familyService) => familyService.SearchFamilyTree(personId))
.WithName("GetFamilyTreeNode")
.WithOpenApi( );

app.Run( );
