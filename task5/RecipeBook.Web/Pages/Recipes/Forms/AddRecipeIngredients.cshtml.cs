using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages.Forms
{
    [BindProperties(SupportsGet = true)]
    public class AddRecipeIngredientsModel : PageModel
    {
        private readonly IngredientController ingredientController;
        private readonly CategoryController categoryController;
        private readonly RecipeController recipeController;

        public Recipe Recipe { get; set; }

        public int RecipeId { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
        public string Ingredients { get; set; }

        public List<RecipeIngredient> RecipeIngredients { get; private set; } = new List<RecipeIngredient>();

        public AddRecipeIngredientsModel(IngredientController ingredientController, CategoryController categoryController, RecipeController recipeController)
        {
            this.ingredientController = ingredientController;
            this.categoryController = categoryController;
            this.recipeController = recipeController;
        }

        public void OnGet()
        {
            foreach (var ingredient in Ingredients.GetWords())
            {
                var recipeIngredient = new RecipeIngredient
                {
                    Ingredient = new Ingredient(ingredient)
                };

                RecipeIngredients.Add(recipeIngredient);
            }
        }

        public async Task OnGetRecipeAsync(int recipeId)
        {
            Recipe = await recipeController.GetByIdAsync<Recipe>(recipeId);
            RecipeIngredients = Recipe.RecipeIngredients.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var ingredients = Ingredients.GetWords();
            for (int i = 0; i < RecipeIngredients.Count; i++)
            {
                RecipeIngredients[i].Ingredient = await ingredientController.GetOrCreateIngredientAsync(ingredients[i]);
            }

            if (RecipeId == 0)
            {
                Category category = await categoryController.GetOrCreateCategoryAsync(CategoryName, ParentCategoryName);
                Recipe recipe = await recipeController.GetOrCreateRecipeAsync(Recipe.Name, category, Recipe.Description, RecipeIngredients, Recipe.Instruction, Recipe.DurationInMinutes.Value);

                await recipeController.AddRecipeAsync(recipe);
            }
            else
            {
                var recipe = await recipeController.GetByIdAsync<Recipe>(RecipeId);

                recipe.Name = Recipe.Name;
                recipe.Category.Name = CategoryName;
                recipe.Description = Recipe.Description;
                recipe.Instruction = Recipe.Instruction;

                for (int i = 0; i < recipe.RecipeIngredients.Count; i++)
                {
                    recipe.RecipeIngredients.ToList()[i].Amount = RecipeIngredients[i].Amount;
                }

                recipe.DurationInMinutes = Recipe.DurationInMinutes;

                await categoryController.UpdateCategoryAsync(recipe.Category, ParentCategoryName);
                await recipeController.UpdateAsync(recipe);
            }

            return RedirectToPage("/Recipes/RecipesList");
        }
    }
}