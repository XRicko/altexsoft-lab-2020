using System;

namespace RecipeBook.Core.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IIngredientRepository Ingredients { get; }
        IRecipeRepository Recipes { get; }

        void Save();
    }
}
