using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages.Forms
{
    [BindProperties]
    public class AddRecipeModel : PageModel
    {
        private readonly RecipeController recipeController;

        public string Message { get; private set; } = "Add recipe";

        public Recipe Recipe { get; set; }

        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
        public string Ingredients { get; set; }

        public AddRecipeModel(RecipeController recipeController)
        {
            this.recipeController = recipeController;
        }

        public async Task OnGetRecipeAsync(int id)
        {
            Message = "Update recipe";

            Recipe = await recipeController.GetByIdAsync<Recipe>(id);
            Ingredients = string.Join(", ", Recipe.RecipeIngredients.Select(i => i.Ingredient.Name));
        }

        public IActionResult OnPost()
        {
            return Recipe.Id == 0
                ? RedirectToPage("AddRecipeIngredients", new { Recipe.Name, CategoryName, ParentCategoryName, Recipe.Description, Ingredients, Recipe.Instruction, Recipe.DurationInMinutes })
                : RedirectToPage("AddRecipeIngredients", "Recipe", new { RecipeId = Recipe.Id, Recipe.Name, CategoryName, ParentCategoryName, Recipe.Description, Ingredients, Recipe.Instruction, Recipe.DurationInMinutes });
        }
    }
}