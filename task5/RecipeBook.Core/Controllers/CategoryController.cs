using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public class CategoryController : ControllerBase
    {
        public CategoryController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<Category> GetOrCreateCategoryAsync(string name, int? parentId = null)
        {
            var category = await GetByNameAsync<Category>(name);

            if (!(category is null))
                return category;
            if (parentId.HasValue)
            {
                var parent = await GetByIdAsync<Category>(parentId.Value);
                return new Category(name, parent.Id);
            }

            return new Category(name);
        }

        public Task<IEnumerable<Category>> GetTopCategoriesAsync() =>
            UnitOfWork.Repository.FindAsync<Category>(c => !c.ParentId.HasValue);
    }
}
