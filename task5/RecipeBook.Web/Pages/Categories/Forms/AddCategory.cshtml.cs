using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages.Categories.Forms
{
    [BindProperties]
    public class AddCategoryModel : PageModel
    {
        private readonly CategoryController categoryController;

        public string Message { get; private set; } = "Add category";

        public Category Category { get; set; }
        public string ParentName { get; set; }

        public AddCategoryModel(CategoryController categoryController)
        {
            this.categoryController = categoryController;
        }

        public async Task OnGetCategoryAsync(int id)
        {
            Message = "Update category";
            Category = await categoryController.GetByIdAsync<Category>(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Category.Id == 0)
            {
                Category = await categoryController.GetOrCreateCategoryAsync(Category.Name, ParentName);
                await categoryController.AddItemAsync(Category);
            }
            else
            {
                var category = await categoryController.GetByIdAsync<Category>(Category.Id);
                category.Name = Category.Name;

                await categoryController.UpdateCategoryAsync(category, ParentName);
            }

            return RedirectToPage("/Categories/CategoriesList");
        }
    }
}