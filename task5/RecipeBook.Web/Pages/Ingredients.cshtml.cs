using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages
{
    public class IngredientsModel : PageModel
    {
        private readonly ILogger<IngredientsModel> logger;
        private readonly IngredientController ingredientController;

        public IEnumerable<Ingredient> Ingredients { get; private set; }

        public IngredientsModel(ILogger<IngredientsModel> logger, IngredientController ingredientController)
        {
            this.logger = logger;
            this.ingredientController = ingredientController;
        }

        public async Task OnGetAsync()
        {
            Ingredients = await ingredientController.GetItemsAsync<Ingredient>();
            //Ingredients = Ingredients.OrderBy(i => i.Name);
        }
    }
}