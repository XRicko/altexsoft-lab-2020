using RecipeBook.Core.Entities;
using RecipeBook.Core.Exceptions;
using RecipeBook.Core.Extensions;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
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

        public async Task<Recipe> CreateRecipeAsync(string name, Category category, string desription, List<RecipeIngredient> recipeIngredients, string instruction, double durationInMinutes)
        {
            var standirdizedName = name.StandardizeName();
            var recipe = await UnitOfWork.Repository.GetAsync<Recipe>(standirdizedName);

            if (recipe == null)
                return new Recipe(standirdizedName, category, desription, recipeIngredients, instruction, durationInMinutes);

            return recipe;
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            await CheckRecipeForExistenceAsync(recipe);

            await AddItemAsync(recipe);
            await AddItemAsync(recipe.Category);

            foreach (var recipeIngredient in recipe.RecipeIngredients)
            {
                await AddItemAsync(recipeIngredient.Ingredient);
            }

            await UnitOfWork.SaveAsync();
        }

        private async Task CheckRecipeForExistenceAsync(Recipe recipe)
        {
            var r = await UnitOfWork.Repository.GetAsync(recipe);

            if (r != null)
                throw new RecipeExistsException(recipe);
        }
    }
}
