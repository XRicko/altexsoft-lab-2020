using RecipeBook.SharedKernel;
using System.Collections.Generic;

namespace RecipeBook.Core.Entities
{
    public class Category : BaseEntity
    {
        public int? ParentId { get; private set; }
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; private set; }
        public virtual ICollection<Recipe> Recipes { get; private set; }

        public Category(string name, int? parentId = null) : base(name)
        {
            ParentId = parentId;
        }

        public Category() : base()
        {

        }
    }
}
