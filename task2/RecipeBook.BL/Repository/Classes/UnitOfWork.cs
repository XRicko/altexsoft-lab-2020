using RecipeBook.BL.Controllers;
using RecipeBook.BL.Repository.Interfaces;
using System.Linq;

namespace RecipeBook.BL.Repository.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataManager manager;

        public ICategoryRepository Categories { get; }
        public IIngredientRepository Ingredients { get; }
        public IRecipeRepository Recipes { get; }

        public UnitOfWork(IDataManager manager)
        {
            this.manager = manager;

            Categories = new CategoryRepository(this.manager);
            Ingredients = new IngredientRepository(this.manager);
            Recipes = new RecipeRepository(this.manager);
        }

        public void Save()
        {
            manager.Save(Categories.GetAll().ToList());
            manager.Save(Ingredients.GetAll().ToList());
            manager.Save(Recipes.GetAll().ToList());
        }
    }
}
