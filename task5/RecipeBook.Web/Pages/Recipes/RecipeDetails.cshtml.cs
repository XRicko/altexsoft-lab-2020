using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages
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

        public async Task<PageResult> OnGetAsync(string name)
        {
            Recipe = await recipeController.GetByNameAsync<Recipe>(name);

            return Page();
        }
    }
}