using RecipeBook.Core.Entities;
using RecipeBook.Core.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook.Core.Controllers
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

        public void RemoveIngredient(Ingredient ingredient)
        {
            unitOfWork.Ingredients.Remove(ingredient);
            unitOfWork.Save();
        }

        public void RemoveIngredient(string name)
        {
            StandardizeName(name);
            unitOfWork.Ingredients.Remove(name);

            unitOfWork.Save();
        }

        public List<Ingredient> GetIngredients()
        {
            return unitOfWork.Ingredients.GetAll().ToList();
        }
    }
}
