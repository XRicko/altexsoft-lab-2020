using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages.Recipes
{
    public class AddRecipeModel : PageModel
    {
        private readonly IngredientController ingredientController;
        private readonly CategoryController categoryController;
        private readonly RecipeController recipeController;

        private IEnumerable<Category> categories;
        private List<RecipeIngredient> recipeIngredients;

        [BindProperty]
        public List<Ingredient> Ingredients { get; private set; }
        [BindProperty]
        public List<int> IngredientsChecked { get; set; } = new List<int>();

        public SelectList CategoryOptions { get; private set; }
        [BindProperty]
        public int SelectedCategory { get; private set; }

        [BindProperty]
        public string[] Amounts { get; set; }

        [BindProperty]
        public string Message { get; private set; } = "Add recipe";

        [BindProperty]
        public Recipe Recipe { get; set; }

        public AddRecipeModel(IngredientController ingredientController, CategoryController categoryController, RecipeController recipeController)
        {
            this.ingredientController = ingredientController;
            this.categoryController = categoryController;
            this.recipeController = recipeController;
        }

        public async Task OnGetAsync() => await InitializeData();

        public async Task OnGetRecipeAsync(int id)
        {
            Message = "Update recipe";

            await InitializeData();

            Recipe = await recipeController.GetByIdAsync<Recipe>(id);
            SelectedCategory = Recipe.CategoryId;

            foreach (var recipeIngredient in Recipe.RecipeIngredients)
            {
                Amounts[recipeIngredient.IngredientId - 1] = recipeIngredient.Amount;
            }

            IngredientsChecked = Recipe.RecipeIngredients.Select(i => i.IngredientId).ToList();
        }

        private async Task InitializeData()
        {
            categories = await categoryController.GetItemsAsync<Category>();
            var ingredients = await ingredientController.GetItemsAsync<Ingredient>();

            Ingredients = ingredients.ToList();
            Amounts = new string[Ingredients.Count];

            CategoryOptions = new SelectList(categories, nameof(Category.Id), nameof(Category.Name));
        }

        public async Task<IActionResult> OnPostAsync(int categoryId)
        {
            var category = await categoryController.GetByIdAsync<Category>(categoryId);
            recipeIngredients = new List<RecipeIngredient>();

            for (int i = 0; i < IngredientsChecked.Count; i++)
            {
                var recipeIngredient = new RecipeIngredient();
                recipeIngredient.Ingredient = await ingredientController.GetByIdAsync<Ingredient>(IngredientsChecked[i]);
                recipeIngredient.Amount = Amounts[IngredientsChecked[i] - 1];

                recipeIngredients.Add(recipeIngredient);
            }

            if (Recipe.Id == 0)
            {
                Recipe recipe = await recipeController.GetOrCreateRecipeAsync(Recipe.Name, category, Recipe.Description, recipeIngredients, Recipe.Instruction, Recipe.DurationInMinutes.Value);

                await recipeController.AddRecipeAsync(recipe);

                return RedirectToPage("RecipesList");
            }
            else
            {
                var recipe = await recipeController.GetByIdAsync<Recipe>(Recipe.Id);

                recipe.Name = Recipe.Name;
                recipe.Category = category;
                recipe.Instruction = Recipe.Instruction;
                recipe.Description = Recipe.Description;
                recipe.DurationInMinutes = Recipe.DurationInMinutes;

                for (int i = 0; i < recipeIngredients.Count; i++)
                {
                    if (i >= recipe.RecipeIngredients.Count)
                        recipe.RecipeIngredients.Add(recipeIngredients[i]);
                    else
                    {
                        recipe.RecipeIngredients.ElementAt(i).Ingredient = recipeIngredients[i].Ingredient;
                        recipe.RecipeIngredients.ElementAt(i).Amount = recipeIngredients[i].Amount;
                    }
                }

                await recipeController.UpdateAsync(recipe);

                return RedirectToPage("RecipeDetails", new { recipe.Id });
            }
        }
    }
}