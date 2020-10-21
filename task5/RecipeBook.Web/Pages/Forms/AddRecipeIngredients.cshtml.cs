using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.Core.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages.Forms
{
    [BindProperties(SupportsGet = true)]
    public class AddRecipeIngredientsModel : PageModel
    {
        private readonly IngredientController ingredientController;
        private readonly CategoryController categoryController;
        private readonly RecipeController recipeController;

        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Instruction { get; set; }
        public double Duration { get; set; }

        public List<RecipeIngredient> RecipeIngredients { get; } = new List<RecipeIngredient>();

        public AddRecipeIngredientsModel(IngredientController ingredientController, CategoryController categoryController, RecipeController recipeController)
        {
            this.ingredientController = ingredientController;
            this.categoryController = categoryController;
            this.recipeController = recipeController;
        }

        public async Task OnGetAsync()
        {
            foreach (var ingredient in Ingredients.GetWords())
            {
                var recipeIngredient = new RecipeIngredient
                {
                    Ingredient = await ingredientController.CreateIngredientAsync(ingredient)
                };

                RecipeIngredients.Add(recipeIngredient);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var ingredients = Ingredients.GetWords();
            for (int i = 0; i < RecipeIngredients.Count; i++)
            {
                RecipeIngredients[i].Ingredient = await ingredientController.CreateIngredientAsync(ingredients[i]);
            }

            Category category = await categoryController.CreateCategoryAsync(CategoryName, ParentCategoryName);
            Recipe recipe = await recipeController.CreateRecipeAsync(Name, category, Description, RecipeIngredients, Instruction, Duration);

            await recipeController.AddRecipeAsync(recipe);

            return RedirectToPage("../Recipes");
        }
    }
}