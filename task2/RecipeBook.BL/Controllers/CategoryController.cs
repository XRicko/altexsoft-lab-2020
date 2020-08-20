using RecipeBook.BL.Models;
using System.Collections.Generic;

namespace RecipeBook.BL.Controllers
{
    public class CategoryController : ControllerBase
    {
        public List<Category> Categories { get; }

        public CategoryController()
        {
            Categories = GetCategories();
        }

        private List<Category> GetCategories()
        {
            return Load<Category>();
        }
        public List<Category> GetCategories(string name)
        {
            return Categories.FindAll(c => c.ParentName == name);
        }
        public Category CreateCategory(string name, string parentName = null)
        {
            var category = GetItem(Categories, ref name);

            if (category == null)
            {
                if (parentName != null)
                {
                    var parent = GetItem(Categories, ref parentName);
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
            AddItem(Categories, category);
        }
    }
}
