using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<IndexModel> logger;
        private readonly RecipeController recipeController;

        public IEnumerable<Recipe> Recipes { get; private set; }

        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        public IndexModel(ILogger<IndexModel> logger, RecipeController recipeController)
        {
            this.logger = logger;
            this.recipeController = recipeController;
        }

        public async Task OnGetAsync()
        {
            Recipes = await recipeController.GetItemsAsync<Recipe>();
        }
    }
}
