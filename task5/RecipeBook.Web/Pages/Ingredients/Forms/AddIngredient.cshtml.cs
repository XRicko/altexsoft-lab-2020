using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages.Ingredients.Forms
{
    public class AddIngredientsModel : PageModel
    {
        private readonly IngredientController ingredientController;

        [BindProperty]
        public string Message { get; private set; } = "Add ingredient";
        [BindProperty]
        public Ingredient Ingredient { get; set; }

        public AddIngredientsModel(IngredientController ingredientController)
        {
            this.ingredientController = ingredientController;
        }

        public async Task OnGetIngredientAsync(string name)
        {
            Message = "Update ingredient";
            Ingredient = await ingredientController.GetByNameAsync<Ingredient>(name);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Ingredient.Id == 0)
                await ingredientController.AddItemAsync(new Ingredient(Ingredient.Name));
            else
            {
                var ingredient = await ingredientController.GetByIdAsync<Ingredient>(Ingredient.Id);
                ingredient.Name = Ingredient.Name;

                await ingredientController.UpdateAsync(ingredient);
            }

            return RedirectToPage("/Ingredients/IngredientsList");
        }
    }
}