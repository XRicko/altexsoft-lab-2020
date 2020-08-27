using RecipeBook.BL.Models;
using RecipeBook.BL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook.BL.Controllers
{
    public class RecipeController : ControllerBase
    {
        public RecipeController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public List<Recipe> GetRecipesInCategory(string name)
        {
            return unitOfWork.Recipes.Find(r => r.Category.Name == name).ToList();
        }
        public Recipe CreateRecipe(string name, Category category, string desription, List<RecipeIngredient> ingredients, string[] instruction, double durationInMinutes)
        {
            name = StandardizeName(name);
            var recipe = unitOfWork.Recipes.Get(name);

            if (recipe == null)
                return new Recipe(name, category, desription, ingredients, instruction, durationInMinutes);

            return recipe;
        }
        public void AddRecipe(Recipe recipe)
        {
            CheckRecipeForExistence(recipe);

            unitOfWork.Recipes.Add(recipe);
            unitOfWork.Categories.Add(recipe.Category);

            foreach (var recipeIngredient in recipe.Ingredients)
            {
                unitOfWork.Ingredients.Add(recipeIngredient.Ingredient);
            }

            unitOfWork.Save();
        }
        private void CheckRecipeForExistence(Recipe recipe)
        {
            if (unitOfWork.Recipes.Get(recipe) != null)
                throw new ArgumentException("This recipe already exists", nameof(recipe));
        }
    }
}
