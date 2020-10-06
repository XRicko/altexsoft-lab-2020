using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBook.Core.Entities;

namespace RecipeBook.Infrastructure.Data.Config
{
    class RecipeIngredientConfig : IEntityTypeConfiguration<RecipeIngredient>
    {
        public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
        {
            builder.Property(e => e.Amount)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.HasOne(d => d.Ingredient)
                .WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Recipe)
                .WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
