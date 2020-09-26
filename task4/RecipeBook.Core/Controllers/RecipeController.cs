using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public class RecipeController : ControllerBase
    {
        public RecipeController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Task<IEnumerable<Recipe>> GetRecipesInCategoryAsync(string name)
        {
            return UnitOfWork.Repository.FindAsync<Recipe>(r => r.Category.Name == StandardizeName(name));
        }

        public async Task<Recipe> CreateRecipeAsync(string name, Category category, string desription, List<RecipeIngredient> recipeIngredients, string instruction, double durationInMinutes)
        {
            var standirdizedName = StandardizeName(name);
            var recipe = await UnitOfWork.Repository.GetAsync<Recipe>(standirdizedName);

            if (recipe == null)
                return new Recipe(standirdizedName, category, desription, recipeIngredients, instruction, durationInMinutes);

            return recipe;
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            await CheckRecipeForExistenceAsync(recipe);

            await AddAsync(recipe);
            await AddAsync(recipe.Category);

            foreach (var recipeIngredient in recipe.RecipeIngredient)
            {
                await AddAsync(recipeIngredient.Ingredient);
            }

            UnitOfWork.SaveAsync();
        }

        private async Task CheckRecipeForExistenceAsync(Recipe recipe)
        {
            var r = await UnitOfWork.Repository.GetAsync(recipe);

            if (r != null)
                throw new ArgumentException("This recipe already exists", nameof(recipe));
        }
    }
}
