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
            name = StandardizeName(name);
            var category = await unitOfWork.Repository.GetAsync<Category>(name);

            if (category != null)
                return category;
            if (!string.IsNullOrWhiteSpace(parentName))
            {
                parentName = StandardizeName(parentName);
                name = name + " " + parentName;

                var parent = await unitOfWork.Repository.GetAsync<Category>(parentName);

                if (parent == null)
                {
                    parent = new Category(parentName);
                    await AddAsync(parent);
                }

                return new Category(name, parent.Id);
            }

            return new Category(name);
        }

        public async Task<List<Category>> GetCategoriesAsync(int? id)
        {
            var categories = await unitOfWork.Repository.FindAsync<Category>(c => c.ParentId == id);
            return categories.ToList();
        }
    }
}
