using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using System.Threading.Tasks;

namespace RecipeBook.Web.Pages.Categories
{
    [BindProperties]
    public class AddCategoryModel : PageModel
    {
        private readonly CategoryController categoryController;

        public string Message { get; private set; } = "Add category";

        public Category Category { get; set; }
        public int ParentId { get; set; }

        public AddCategoryModel(CategoryController categoryController)
        {
            this.categoryController = categoryController;
        }

        public void OnGetAdd(string parentName, int parentId)
        {
            Message += $" to {parentName}";
            ParentId = parentId;
        }

        public async Task OnGetEditAsync(int id)
        {
            Message = "Update category";
            Category = await categoryController.GetByIdAsync<Category>(id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Category.Id == 0)
            {
                Category = await categoryController.GetOrCreateCategoryAsync(Category.Name, ParentId);
                await categoryController.AddItemAsync(Category);
            }
            else
            {
                var category = await categoryController.GetByIdAsync<Category>(Category.Id);
                category.Name = Category.Name;

                await categoryController.UpdateAsync(category);
            }

            return RedirectToPage("/Categories/CategoriesList");
        }
    }
}