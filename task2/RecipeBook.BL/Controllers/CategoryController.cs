using RecipeBook.BL.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeBook.BL.Controllers
{
    public class CategoryController : ControllerBase
    {
        public Category CreateCategory(string name, string parentName = null)
        {
            var category = unitOfWork.Categories.Get(ref name);

            if (category == null)
            {
                if (parentName != null)
                {
                    var parent = unitOfWork.Categories.Get(ref parentName);
                    name = name + " " + parentName;

                    if (parent == null)
                    {
                        parent = new Category(parentName);
                        AddCategory(parent);
                    }

                    return new Category(name, parent.Id, parent.Name);
                }

                return new Category(name);
            }

            return category;
        }
        public void AddCategory(Category category)
        {
            unitOfWork.Categories.Add(category);
        }
        public List<Category> GetCategories(string name)
        {
            return unitOfWork.Categories.Find(c => c.ParentName == name).ToList();
        }
    }
}
