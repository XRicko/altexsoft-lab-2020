using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RecipeController recipeController;

        public IEnumerable<Recipe> Recipes { get; private set; }

        public IndexModel(RecipeController recipeController)
        {
            this.recipeController = recipeController;
        }

        public async Task OnGetAsync()
        {
            Recipes = await recipeController.GetItemsAsync<Recipe>();
        }
    }
}
