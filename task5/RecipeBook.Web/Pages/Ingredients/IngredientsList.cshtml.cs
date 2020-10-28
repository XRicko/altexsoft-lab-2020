using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages
{
    public class IngredientsListModel : PageModel
    {
        private readonly ILogger<IngredientsListModel> logger;
        private readonly IngredientController ingredientController;

        public IEnumerable<Ingredient> Ingredients { get; private set; }

        public IngredientsListModel(ILogger<IngredientsListModel> logger, IngredientController ingredientController)
        {
            this.logger = logger;
            this.ingredientController = ingredientController;
        }

        public async Task OnGetAsync()
        {
            Ingredients = await ingredientController.GetItemsAsync<Ingredient>();
        }
    }
}