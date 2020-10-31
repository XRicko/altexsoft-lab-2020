using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Interfaces;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public class IngredientController : ControllerBase
    {
        public IngredientController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<Ingredient> GetOrCreateIngredientAsync(string name)
        {
            var ingredient = await GetByNameAsync<Ingredient>(name);

            if (ingredient is null)
                return new Ingredient(name);

            return ingredient;
        }
    }
}
