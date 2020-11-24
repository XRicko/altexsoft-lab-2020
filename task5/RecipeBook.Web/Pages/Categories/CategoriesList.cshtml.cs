using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages.Categories
{
    public class CategoriesListModel : PageModel
    {
        private readonly ILogger<CategoriesListModel> logger;
        private readonly CategoryController categoryController;

        public string Message { get; set; } = "Categories";

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

        public async Task<IActionResult> OnGetSubcategoriesAsync(int id)
        {
            var category = await categoryController.GetByIdAsync<Category>(id);
            Categories = category.Children;

            Message += $" in {category.Name}";

            return Page();
        }
    }
}