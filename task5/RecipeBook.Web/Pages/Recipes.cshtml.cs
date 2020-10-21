using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages
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

        public async Task OnGetWithIngredientAsync(string ingredientName)
        {
            Recipes = await recipeController.GetRecipesWithIngredientAsync(ingredientName);
            Message = $"Recipes with {ingredientName}";
        }

        public async Task OnGetInCategory(string categoryName)
        {
            Recipes = await recipeController.GetRecipesInCategoryAsync(categoryName);
            Message = $"Recipes in category {categoryName}";
        }
    }
}