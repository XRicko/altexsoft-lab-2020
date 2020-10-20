using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages
{
    public class CategoriesModel : PageModel
    {
        private readonly ILogger<CategoriesModel> logger;
        private readonly CategoryController categoryController;

        public IEnumerable<Category> Categories { get; private set; }

        public CategoriesModel(ILogger<CategoriesModel> logger, CategoryController categoryController)
        {
            this.logger = logger;
            this.categoryController = categoryController;
        }

        public async Task OnGetAsync()
        {
            Categories = await categoryController.GetTopCategoriesAsync();
        }

        public async Task<IActionResult> OnGetSubcategoriesAsync(string categoryName)
        {
            var category = await categoryController.GetByNameAsync<Category>(categoryName);
            Categories = category.Children;

            if (!Categories.Any())
                return RedirectToPage("Recipes", "InCategory", new { categoryName });

            return Page();
        }
    }
}