﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecipeBook.Infrastructure.Data;

namespace RecipeBook.Infrastructure.Migrations
{
    [DbContext(typeof(RecipeBookContext))]
    [Migration("20200919210130_CreateRecipeBookDB")]
    partial class CreateRecipeBookDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RecipeBook.Core.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(75)")
                        .HasMaxLength(75);

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("UQ__Category__8517B2E01CAD4794");

                    b.HasIndex("ParentId");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("RecipeBook.Core.Entities.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(75)")
                        .HasMaxLength(75);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("UQ__Ingredie__A1B2F1CC445D2F03");

                    b.ToTable("Ingredient");
                });

            modelBuilder.Entity("RecipeBook.Core.Entities.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<double?>("DurationInMinutes")
                        .HasColumnType("float");

                    b.Property<string>("Instruction")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(75)")
                        .HasMaxLength(75);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("UQ__Recipe__9EFE16E92347DBFB");

                    b.ToTable("Recipe");
                });

            modelBuilder.Entity("RecipeBook.Core.Entities.RecipeIngredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("IngredientId")
                        .HasColumnType("int");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IngredientId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeIngredient");
                });

            modelBuilder.Entity("RecipeBook.Core.Entities.Category", b =>
                {
                    b.HasOne("RecipeBook.Core.Entities.Category", "Parent")
                        .WithMany("InverseParent")
                        .HasForeignKey("ParentId")
                        .HasConstraintName("FK__Category__Parent__28B808A7");
                });

            modelBuilder.Entity("RecipeBook.Core.Entities.Recipe", b =>
                {
                    b.HasOne("RecipeBook.Core.Entities.Category", "Category")
                        .WithMany("Recipe")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK__Recipe__Category__2C88998B")
                        .IsRequired();
                });

            modelBuilder.Entity("RecipeBook.Core.Entities.RecipeIngredient", b =>
                {
                    b.HasOne("RecipeBook.Core.Entities.Ingredient", "Ingredient")
                        .WithMany("RecipeIngredient")
                        .HasForeignKey("IngredientId")
                        .HasConstraintName("FK__RecipeIng__Ingre__30592A6F")
                        .IsRequired();

                    b.HasOne("RecipeBook.Core.Entities.Recipe", "Recipe")
                        .WithMany("RecipeIngredient")
                        .HasForeignKey("RecipeId")
                        .HasConstraintName("FK__RecipeIng__Recip__2F650636")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
