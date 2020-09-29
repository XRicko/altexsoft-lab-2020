using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBook.Core.Entities;

namespace RecipeBook.Infrastructure.Data.Config
{
    class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasIndex(e => e.Name)
                    .IsUnique();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(75);

            builder.HasOne(d => d.Children)
                .WithMany(p => p.InverseParents)
                .HasForeignKey(d => d.ParentId);
        }
    }
}
