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
        private readonly CategoryController categoryController;

        public string Message { get; private set; } = "Recipes";

        public IEnumerable<Recipe> Recipes { get; private set; }

        public RecipesListModel(ILogger<RecipesListModel> logger, RecipeController recipeController, CategoryController categoryController)
        {
            this.logger = logger;

            this.recipeController = recipeController;
            this.categoryController = categoryController;
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

        public async Task OnGetInCategory(int categoryId)
        {
            var category = await categoryController.GetByIdAsync<Category>(categoryId);
            Recipes = category.Recipes;

            Message += $" in category {category.Name}";
        }
    }
}