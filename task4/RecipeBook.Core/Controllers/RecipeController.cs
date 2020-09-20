using RecipeBook.Core.Entities;
using RecipeBook.Core.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook.Core.Controllers
{
    public class RecipeController : ControllerBase
    {
        public RecipeController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public List<Recipe> GetRecipesInCategory(string name)
        {
            return unitOfWork.Recipes.Find(r => r.Category.Name == name).ToList();
        }

        public Recipe CreateRecipe(string name, Category category, string desription, List<RecipeIngredient> recipeIngredients, string instruction, double durationInMinutes)
        {
            name = StandardizeName(name);
            var recipe = unitOfWork.Recipes.Get(name);

            if (recipe == null)
                return new Recipe(name, category, desription, recipeIngredients, instruction, durationInMinutes);

            return recipe;
        }

        public void AddRecipe(Recipe recipe)
        {
            CheckRecipeForExistence(recipe);

            unitOfWork.Recipes.Add(recipe);
            unitOfWork.Categories.Add(recipe.Category);

            foreach (var recipeIngredient in recipe.RecipeIngredient)
            {
                unitOfWork.Ingredients.Add(recipeIngredient.Ingredient);
            }

            unitOfWork.Save();
        }

        public void RemoveRecipe(Recipe recipe)
        {
            unitOfWork.Recipes.Remove(recipe);
            unitOfWork.Save();
        }

        public void RemoveRecipe(string name)
        {
            StandardizeName(name);
            unitOfWork.Recipes.Remove(name);

            unitOfWork.Save();
        }

        private void CheckRecipeForExistence(Recipe recipe)
        {
            if (unitOfWork.Recipes.Get(recipe) != null)
                throw new ArgumentException("This recipe already exists", nameof(recipe));
        }
    }
}
