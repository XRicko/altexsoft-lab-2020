using RecipeBook.BL.Models;
using RecipeBook.BL.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook.BL.Controllers
{
    public class IngredientController : ControllerBase
    {
        public IngredientController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Ingredient CreateIngredient(string name)
        {
            name = StandardizeName(name);
            var ingredient = unitOfWork.Ingredients.Get(name);

            if (ingredient == null)
                return new Ingredient(name);

            return ingredient;
        }
        public void AddIngredient(Ingredient ingredient)
        {
            if (unitOfWork.Ingredients.Get(ingredient) == null)
                unitOfWork.Ingredients.Add(ingredient);
        }
        public List<Ingredient> GetIngredients()
        {
            return unitOfWork.Ingredients.GetAll().ToList();
        }
    }
}
