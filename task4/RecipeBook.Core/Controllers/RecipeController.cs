using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public class RecipeController : ControllerBase
    {
        public RecipeController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<List<Recipe>> GetRecipesInCategoryAsync(string name)
        {
            var recipes = await unitOfWork.Repository.FindAsync<Recipe>(r => r.Category.Name == name);
            return recipes.ToList();
        }

        public async Task<Recipe> CreateRecipeAsync(string name, Category category, string desription, List<RecipeIngredient> recipeIngredients, string instruction, double durationInMinutes)
        {
            name = StandardizeName(name);
            var recipe = await unitOfWork.Repository.GetAsync<Recipe>(name);

            if (recipe == null)
                return new Recipe(name, category, desription, recipeIngredients, instruction, durationInMinutes);

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

            unitOfWork.SaveAsync();
        }

        private async Task CheckRecipeForExistenceAsync(Recipe recipe)
        {
            var r = await unitOfWork.Repository.GetAsync(recipe);

            if (r != null)
                throw new ArgumentException("This recipe already exists", nameof(recipe));
        }
    }
}
