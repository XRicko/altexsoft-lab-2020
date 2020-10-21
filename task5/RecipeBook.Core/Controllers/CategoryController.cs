using RecipeBook.Core.Entities;
using RecipeBook.Core.Extensions;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public class CategoryController : ControllerBase
    {
        public CategoryController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<Category> CreateCategoryAsync(string name, string parentName = null)
        {
            string standardizedName = name.StandardizeName();
            var category = await UnitOfWork.Repository.GetAsync<Category>(standardizedName);

            if (category != null)
                return category;
            if (!string.IsNullOrWhiteSpace(parentName))
            {
                string standardizedParentName = parentName.StandardizeName();
                string subcategoryName = standardizedName + " " + standardizedParentName;
                string noDublicatesSubcategoryName = subcategoryName.RemoveDublicates();

                var parent = await UnitOfWork.Repository.GetAsync<Category>(standardizedParentName);

                if (parent == null)
                {
                    parent = new Category(standardizedParentName);
                    await AddItemAsync(parent);
                }

                return new Category(noDublicatesSubcategoryName, parent.Id);
            }

            return new Category(standardizedName);
        }

        public Task<IEnumerable<Category>> GetTopCategoriesAsync()
        {
            return UnitOfWork.Repository.FindAsync<Category>(c => c.ParentId == null);
        }
    }
}
