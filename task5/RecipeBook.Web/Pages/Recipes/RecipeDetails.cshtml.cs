using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages.Recipes
{
    public class RecipeDetailsModel : PageModel
    {
        [BindProperty]
        public Recipe Recipe { get; set; }

        private readonly RecipeController recipeController;

        public RecipeDetailsModel(RecipeController recipeController)
        {
            this.recipeController = recipeController;
        }

        public async Task<PageResult> OnGetAsync(int id)
        {
            Recipe = await recipeController.GetByIdAsync<Recipe>(id);

            return Page();
        }
    }
}