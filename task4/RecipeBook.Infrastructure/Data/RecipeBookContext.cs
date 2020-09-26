using Microsoft.EntityFrameworkCore;
using RecipeBook.Core.Entities;

namespace RecipeBook.Infrastructure.Data
{
    public partial class RecipeBookContext : DbContext
    {
        public RecipeBookContext() { }

        public RecipeBookContext(DbContextOptions<RecipeBookContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }
        public virtual DbSet<RecipeIngredient> RecipeIngredient { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Category__8517B2E01CAD4794")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK__Category__Parent__28B808A7");
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Ingredie__A1B2F1CC445D2F03")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75);
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Recipe__9EFE16E92347DBFB")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Instruction)
                    .HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Recipe)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Recipe__Category__2C88998B");
            });

            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                entity.Property(e => e.Amount)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.RecipeIngredient)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RecipeIng__Ingre__30592A6F");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipeIngredient)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RecipeIng__Recip__2F650636");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
