using RecipeBook.BL.Controllers;
using RecipeBook.BL.Repository.Interfaces;
using System.Linq;

namespace RecipeBook.BL.Repository.Classes
{
    class UnitOfWork : IUnitOfWork
    {
        private readonly IDataManager manager = new JSONDataManager();

        public ICategoryRepository Categories { get; }
        public IIngredientRepository Ingredients { get; }
        public IRecipeRepository Recipes { get; }

        public UnitOfWork()
        {
            Categories = new CategoryRepository();
            Ingredients = new IngredientRepository();
            Recipes = new RecipeRepository();
        }

        public void Save()
        {
            manager.Save(Categories.GetAll().ToList());
            manager.Save(Ingredients.GetAll().ToList());
            manager.Save(Recipes.GetAll().ToList());
        }
    }
}
