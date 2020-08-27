using RecipeBook.BL.Controllers;
using RecipeBook.BL.Models;
using RecipeBook.BL.Repository.Interfaces;

namespace RecipeBook.BL.Repository.Classes
{
    class RecipeRepository : Repository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(IDataManager manager) : base(manager) { }
    }
}
