using Microsoft.EntityFrameworkCore;
using RecipeBook.Core.Entities;
using RecipeBook.Infrastructure.Data.Config;

namespace RecipeBook.Infrastructure.Data
{
    public class RecipeBookContext : DbContext
    {
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
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new IngredientConfig());
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new RecipeConfig());
            modelBuilder.ApplyConfiguration(new RecipeIngredientConfig());
        }
    }
}
