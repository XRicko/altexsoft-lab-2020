using RecipeBook.Core.Entities;
using RecipeBook.Core.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook.Core.Controllers
{
    public class CategoryController : ControllerBase
    {
        public CategoryController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Category CreateCategory(string name, string parentName = null)
        {
            name = StandardizeName(name);
            var category = unitOfWork.Categories.Get(name);

            if (category != null)
                return category;
            if (!string.IsNullOrWhiteSpace(parentName))
            {
                parentName = StandardizeName(parentName);
                name = name + " " + parentName;

                var parent = unitOfWork.Categories.Get(parentName);

                if (parent == null)
                {
                    parent = new Category(parentName);
                    AddCategory(parent);
                }

                return new Category(name, parent.Id);
            }

            return new Category(name);
        }

        public void AddCategory(Category category)
        {
            if (unitOfWork.Categories.Get(category) == null)
                unitOfWork.Categories.Add(category);
        }

        public void RemoveCategory(Category category)
        {
            unitOfWork.Categories.Remove(category);
            unitOfWork.Save();
        }

        public void RemoveCategory(string name)
        {
            StandardizeName(name);
            unitOfWork.Categories.Remove(name);

            unitOfWork.Save();
        }

        public List<Category> GetCategories(int? id)
        {
            return unitOfWork.Categories.Find(c => c.ParentId == id).ToList();
        }
    }
}
