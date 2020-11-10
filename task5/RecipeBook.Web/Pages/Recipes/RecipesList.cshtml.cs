using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages.Recipes
{
    public class RecipesListModel : PageModel
    {
        private readonly ILogger<RecipesListModel> logger;
        private readonly RecipeController recipeController;

        public string Message { get; private set; } = "Recipes";

        public IEnumerable<Recipe> Recipes { get; private set; }

        public RecipesListModel(ILogger<RecipesListModel> logger, RecipeController recipeController)
        {
            this.logger = logger;
            this.recipeController = recipeController;
        }

        public async Task OnGetAsync()
        {
            Recipes = await recipeController.GetItemsAsync<Recipe>();
        }

        public async Task OnGetWithIngredientAsync(int ingredientId, string ingredientName)
        {
            Recipes = await recipeController.GetRecipesWithIngredientAsync(ingredientId);
            Message += $" with {ingredientName}";
        }

        public async Task OnGetInCategory(int categoryId, string categoryName)
        {
            Recipes = await recipeController.GetRecipesInCategoryAsync(categoryId);
            Message += $" in category {categoryName}";
        }
    }
}