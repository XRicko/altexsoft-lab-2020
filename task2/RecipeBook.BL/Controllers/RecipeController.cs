using RecipeBook.BL.Models;
using System;
using System.Collections.Generic;

namespace RecipeBook.BL.Controllers
{
    public class RecipeController : ControllerBase
    {
        public List<Recipe> Recipes { get; }

        private readonly CategoryController categoryController;
        private readonly IngredientController ingredientController;

        public RecipeController(CategoryController categoryController, IngredientController ingredientController)
        {
            Recipes = GetRecipes();
            this.categoryController = categoryController;
            this.ingredientController = ingredientController;
        }

        private List<Recipe> GetRecipes()
        {
            return Load<Recipe>();
        }
        public List<Recipe> GetRecipesInCategory(string name)
        {
            return Recipes.FindAll(r => r.Category.Name == name);
        }
        public Recipe CreateRecipe(string name, Category category, string desription, List<RecipeIngredient> ingredients, string[] instruction, double durationInMinutes)
        {
            var recipe = GetItem(Recipes, ref name);

            if (recipe == null)
                return new Recipe(name, category, desription, ingredients, instruction, durationInMinutes);

            return recipe;
        }
        public void AddRecipe(Recipe recipe)
        {
            bool isAdded = AddItem(Recipes, recipe);

            if (!isAdded)
                throw new ArgumentException("This recipe already exists", nameof(recipe));

            categoryController.AddCategory(recipe.Category);
            foreach (var recipeIngredient in recipe.Ingredients)
            {
                ingredientController.AddIngredient(recipeIngredient.Ingredient);
            }

            Save();
        }
        private void Save()
        {
            Save(categoryController.Categories);
            Save(Recipes);
            Save(ingredientController.Ingredients);
        }
    }
}
