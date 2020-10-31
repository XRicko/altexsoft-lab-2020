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
            var category = await GetByNameAsync<Category>(FormCategoryName(name, parentName));

            if (category is object)
                return category;
            if (!string.IsNullOrWhiteSpace(parentName))
                return await GetOrCreateSubcategoryAsync(name, parentName);

            return new Category(name);
        }

        private async Task<Category> GetOrCreateSubcategoryAsync(string name, string parentName)
        {
            string noDublicatesSubcategoryName = FormCategoryName(name, parentName);
            Category parent = await GetOrCreateParentCategoryAsync(parentName);

            return new Category(noDublicatesSubcategoryName, parent.Id);
        }

        private string FormCategoryName(string name, string parentName)
        {
            string categoryName = parentName is object ? name.StandardizeName() + " " + parentName.StandardizeName() : name.StandardizeName();
            string noDublicatesCategoryName = categoryName.RemoveDublicates();

            return noDublicatesCategoryName;
        }

        private async Task<Category> GetOrCreateParentCategoryAsync(string parentName)
        {
            var parent = await GetByNameAsync<Category>(parentName);

            if (parent is null)
            {
                parent = new Category(parentName);
                await AddItemAsync(parent);
            }

            return parent;
        }

        public async Task UpdateCategoryAsync(Category category, string parentName = null)
        {
            if (category.Parent is null && !string.IsNullOrWhiteSpace(parentName))
            {
                category.Parent = await GetOrCreateParentCategoryAsync(parentName);
                category.Name = FormCategoryName(category.Name, parentName);
            }

            await UpdateAsync(category);
        }

        public Task<IEnumerable<Category>> GetTopCategoriesAsync() => 
            UnitOfWork.Repository.FindAsync<Category>(c => !c.ParentId.HasValue);
    }
}
