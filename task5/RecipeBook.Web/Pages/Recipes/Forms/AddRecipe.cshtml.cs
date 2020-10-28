using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RecipeBook.Web.Pages.Forms
{
    [BindProperties]
    public class AddRecipeModel : PageModel
    {
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Instruction { get; set; }
        public double Duration { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            return RedirectToPage("AddRecipeIngredients", new { Name, CategoryName, ParentCategoryName, Description, Instruction, Duration, Ingredients });
        }
    }
}