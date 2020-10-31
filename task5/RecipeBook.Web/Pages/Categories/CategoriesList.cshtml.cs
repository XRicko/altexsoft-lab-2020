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
    public class CategoriesListModel : PageModel
    {
        private readonly ILogger<CategoriesListModel> logger;
        private readonly CategoryController categoryController;

        public IEnumerable<Category> Categories { get; private set; }

        public CategoriesListModel(ILogger<CategoriesListModel> logger, CategoryController categoryController)
        {
            this.logger = logger;
            this.categoryController = categoryController;
        }

        public async Task OnGetAsync()
        {
            Categories = await categoryController.GetTopCategoriesAsync();
        }

        public IActionResult OnGetTopCategories(string categoryName)
        {
            return RedirectToRecipes(categoryName);
        }

        public async Task<IActionResult> OnGetSubcategoriesAsync(string categoryName)
        {
            var category = await categoryController.GetByNameAsync<Category>(categoryName);
            Categories = new List<Category> { category };

            if (!category.Children.Any())
                return RedirectToRecipes(categoryName);

            return Page();
        }

        public IActionResult RedirectToRecipes(string categoryName)
        {
            return RedirectToPage("/Recipes/RecipesList", "InCategory", new { categoryName });
        }
    }
}