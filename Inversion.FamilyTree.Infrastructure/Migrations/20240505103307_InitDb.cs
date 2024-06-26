﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inversion.FamilyTree.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitDb : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "People",
			columns: table => new
			{
				Id = table.Column<int>(type: "int", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
				SurName = table.Column<string>(type: "nvarchar(max)", nullable: false),
				BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
				IdentityNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
				FatherId = table.Column<int>(type: "int", nullable: true),
				MotherId = table.Column<int>(type: "int", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_People", x => x.Id);
			});

		migrationBuilder.CreateIndex(
			name: "IX_People_IdentityNumber",
			table: "People",
			column: "IdentityNumber",
			unique: true);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "People");
	}
}
