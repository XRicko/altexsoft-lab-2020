using RecipeBook.SharedKernel;
using System.Collections.Generic;

namespace RecipeBook.Core.Entities
{
    public class Category : BaseEntity
    {
        public int? ParentId { get; private set; }
        public virtual Category Parent { get; private set; }
        public virtual ICollection<Category> InverseParent { get; private set; }
        public virtual ICollection<Recipe> Recipe { get; private set; }

        public Category(string name, int? parentId = null) : base(name)
        {
            ParentId = parentId;
        }
    }
}
