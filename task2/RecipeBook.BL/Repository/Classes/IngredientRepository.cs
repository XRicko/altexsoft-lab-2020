using RecipeBook.BL.Models;
using RecipeBook.BL.Repository.Interfaces;

namespace RecipeBook.BL.Repository.Classes
{
    class IngredientRepository : Repository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository() : base() { }
    }
}
