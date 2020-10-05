using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public class CategoryController : ControllerBase
    {
        public CategoryController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<Category> CreateCategoryAsync(string name, string parentName = null)
        {
            var standardizedName = StandardizeName(name);
            var category = await UnitOfWork.Repository.GetAsync<Category>(standardizedName);

            if (category != null)
                return category;
            if (!string.IsNullOrWhiteSpace(parentName))
            {
                var standardizedParentName = StandardizeName(parentName);
                var subCategoryName = standardizedName + " " + standardizedParentName;
                var noDublicatesSubName = string.Join(" ", subCategoryName.Split(' ').Distinct());

                var parent = await UnitOfWork.Repository.GetAsync<Category>(standardizedParentName);

                if (parent == null)
                {
                    parent = new Category(standardizedParentName);
                    await AddItemAsync(parent);
                }

                return new Category(noDublicatesSubName, parent.Id);
            }

            return new Category(standardizedName);
        }

        public Task<IEnumerable<Category>> GetTopCategoriesAsync()
        {
            return UnitOfWork.Repository.FindAsync<Category>(c => c.ParentId == null);
        }
    }
}
