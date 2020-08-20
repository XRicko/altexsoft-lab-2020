using RecipeBook.BL.Models;
using System.Collections.Generic;

namespace RecipeBook.BL.Controllers
{
    public class IngredientController : ControllerBase
    {
        public List<Ingredient> Ingredients { get; }

        public IngredientController()
        {
            Ingredients = GetIngredients();
        }

        private List<Ingredient> GetIngredients()
        {
            return Load<Ingredient>();
        }
        public Ingredient CreateIngredient(string name)
        {
            var ingredient = GetItem(Ingredients, ref name);

            if (ingredient == null)
                return new Ingredient(name);

            return ingredient;
        }
        public void AddIngredient(Ingredient ingredient)
        {
            AddItem(Ingredients, ingredient);
        }
    }
}
