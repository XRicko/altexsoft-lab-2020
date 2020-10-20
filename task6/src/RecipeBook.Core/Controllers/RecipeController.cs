using RecipeBook.Core.Entities;
using RecipeBook.Core.Exceptions;
using RecipeBook.Core.Extensions;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public class RecipeController : ControllerBase
    {
        public RecipeController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Task<IEnumerable<Recipe>> GetRecipesWithoutCategoryAsync()
        {
            return UnitOfWork.Repository.FindAsync<Recipe>(r => r.Category == null);
        }

        public Task<IEnumerable<Recipe>> GetRecipesInCategoryAsync(string categoryName)
        {
            return UnitOfWork.Repository.FindAsync<Recipe>(r => r.Category.Name == categoryName.StandardizeName());
        }

        public async Task<IEnumerable<Recipe>> GetRecipesWithIngredientAsync(string ingredientName)
        {
            var recipes = await GetItemsAsync<Recipe>();
            var recipesWithIngredient = recipes.SelectMany(r => r.RecipeIngredients)
                .Where(ri => ri.Ingredient.Name == ingredientName.StandardizeName())
                .Select(ri => ri.Recipe);

            return recipesWithIngredient;
        }

        public async Task<Recipe> CreateRecipeAsync(string name, Category category, string desription, List<RecipeIngredient> recipeIngredients, string instruction, double durationInMinutes)
        {
            string standirdizedName = name.StandardizeName();
            var recipe = await UnitOfWork.Repository.GetAsync<Recipe>(standirdizedName);

            if (recipe == null)
                return new Recipe(standirdizedName, category, desription, recipeIngredients, instruction, durationInMinutes);

            return recipe;
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            await CheckRecipeForExistenceAsync(recipe);

            foreach (var recipeIngredient in recipe.RecipeIngredients)
            {
                await AddItemAsync(recipeIngredient.Ingredient);
            }

            await AddItemAsync(recipe.Category);
            await AddItemAsync(recipe);
        }

        private async Task CheckRecipeForExistenceAsync(Recipe recipe)
        {
            Recipe r = await UnitOfWork.Repository.GetAsync(recipe);

            if (r != null)
                throw new RecipeExistsException(recipe);
        }
    }
}
