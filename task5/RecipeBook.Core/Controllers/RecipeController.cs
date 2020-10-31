using RecipeBook.Core.Entities;
using RecipeBook.Core.Exceptions;
using RecipeBook.SharedKernel.Extensions;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public class RecipeController : ControllerBase
    {
        public RecipeController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<IEnumerable<Recipe>> GetRecipesWithIngredientAsync(string ingredientName)
        {
            var recipes = await GetItemsAsync<Recipe>();
            var recipesWithIngredient = recipes.SelectMany(r => r.RecipeIngredients)
                .Where(ri => ri.Ingredient.Name == ingredientName.StandardizeName())
                .Select(ri => ri.Recipe);

            return recipesWithIngredient;
        }

        public async Task<Recipe> GetOrCreateRecipeAsync(string name, Category category, string desription, List<RecipeIngredient> recipeIngredients, string instruction, double durationInMinutes)
        {
            var recipe = await GetByNameAsync<Recipe>(name);

            if (recipe is null)
                return new Recipe(name, category, desription, recipeIngredients, instruction, durationInMinutes);

            return recipe;
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            await CheckRecipeForExistenceAsync(recipe);

            foreach (var recipeIngredient in recipe.RecipeIngredients)
            {
                await AddItemAsync(recipeIngredient.Ingredient);
            }

            Category category = await UnitOfWork.Repository.GetAsync(recipe.Category);

            if (category is object)
                recipe.Category = category;

            await AddItemAsync(recipe.Category);
            await AddItemAsync(recipe);
        }

        private async Task CheckRecipeForExistenceAsync(Recipe recipe)
        {
            Recipe r = await UnitOfWork.Repository.GetAsync(recipe);

            if (r is object)
                throw new RecipeExistsException(recipe);
        }

        public Task<IEnumerable<Recipe>> GetRecipesWithoutCategoryAsync() =>
            UnitOfWork.Repository.FindAsync<Recipe>(r => r.Category == null);

        public Task<IEnumerable<Recipe>> GetRecipesInCategoryAsync(string categoryName) =>
            UnitOfWork.Repository.FindAsync<Recipe>(r => r.Category.Name == categoryName.StandardizeName());

    }
}
