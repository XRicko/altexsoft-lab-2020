using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Interfaces;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public class IngredientController : ControllerBase
    {
        public IngredientController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<Ingredient> CreateIngredientAsync(string name)
        {
            name = StandardizeName(name);
            var ingredient = await unitOfWork.Repository.GetAsync<Ingredient>(name);

            if (ingredient == null)
                return new Ingredient(name);

            return ingredient;
        }
    }
}
