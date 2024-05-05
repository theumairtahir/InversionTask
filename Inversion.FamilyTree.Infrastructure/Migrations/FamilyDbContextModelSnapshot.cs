﻿// <auto-generated />
using System;
using Inversion.FamilyTree.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Inversion.FamilyTree.Infrastructure.Migrations
{
    [DbContext(typeof(FamilyDbContext))]
    partial class FamilyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Inversion.FamilyTree.Domain.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<int?>("FatherId")
                        .HasColumnType("int");

                    b.Property<string>("IdentityNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FatherId");

                    b.HasIndex("IdentityNumber")
                        .IsUnique();

                    b.ToTable("People");
                });

            modelBuilder.Entity("Inversion.FamilyTree.Domain.Entities.Person", b =>
                {
                    b.HasOne("Inversion.FamilyTree.Domain.Entities.Person", "Father")
                        .WithMany()
                        .HasForeignKey("FatherId");

                    b.HasOne("Inversion.FamilyTree.Domain.Entities.Person", "Mother")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Father");

                    b.Navigation("Mother");
                });
#pragma warning restore 612, 618
        }
    }
}
