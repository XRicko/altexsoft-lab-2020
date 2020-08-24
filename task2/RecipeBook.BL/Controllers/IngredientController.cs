using RecipeBook.BL.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook.BL.Controllers
{
    public class IngredientController : ControllerBase
    {
        public Ingredient CreateIngredient(string name)
        {
            var ingredient = unitOfWork.Ingredients.Get(ref name);

            if (ingredient == null)
                return new Ingredient(name);

            return ingredient;
        }
        public void AddIngredient(Ingredient ingredient)
        {
            unitOfWork.Ingredients.Add(ingredient);
        }
        public List<Ingredient> GetIngredients()
        {
            return unitOfWork.Ingredients.GetAll().ToList();
        }
    }
}
