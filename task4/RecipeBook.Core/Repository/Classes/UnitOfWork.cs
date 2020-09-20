using Microsoft.EntityFrameworkCore;
using RecipeBook.Core.Repository.Interfaces;

namespace RecipeBook.Core.Repository.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        public ICategoryRepository Categories { get; }
        public IIngredientRepository Ingredients { get; }
        public IRecipeRepository Recipes { get; }

        public UnitOfWork(DbContext context)
        {
            this.context = context;

            Categories = new CategoryRepository(this.context);
            Ingredients = new IngredientRepository(this.context);
            Recipes = new RecipeRepository(this.context);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
