using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Extensions;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public class CategoryController : ControllerBase
    {
        public CategoryController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<Category> GetOrCreateCategoryAsync(string name, string parentName = null)
        {
            string categoryName = parentName is null ? name.StandardizeName() : name.StandardizeName() + " " + parentName.StandardizeName();
            string noDublicatesCategoryName = categoryName.RemoveDublicates();

            var category = await GetByNameAsync<Category>(noDublicatesCategoryName);

            if (!(category is null))
                return category;
            if (!string.IsNullOrWhiteSpace(parentName))
            {
                var parent = await GetByNameAsync<Category>(parentName);

                if (parent is null)
                {
                    parent = new Category(parentName);
                    await AddItemAsync(parent);
                }

                return new Category(noDublicatesCategoryName, parent.Id);
            }

            return new Category(name);
        }

        public Task<IEnumerable<Category>> GetTopCategoriesAsync() => 
            UnitOfWork.Repository.FindAsync<Category>(c => !c.ParentId.HasValue);
    }
}
