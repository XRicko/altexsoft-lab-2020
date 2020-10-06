using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBook.Core.Entities;

namespace RecipeBook.Infrastructure.Data.Config
{
    class IngredientConfig : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.HasIndex(e => e.Name)
                   .IsUnique();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(75);
        }
    }
}
