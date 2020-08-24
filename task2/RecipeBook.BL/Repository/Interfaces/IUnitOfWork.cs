namespace RecipeBook.BL.Repository.Interfaces
{
    interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IIngredientRepository Ingredients { get; }
        IRecipeRepository Recipes { get; }

        void Save();
    }
}
