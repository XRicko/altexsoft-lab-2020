using System.Collections.Generic;

namespace RecipeBook.Core.Entities
{
    public partial class Category : EntityBase
    {
        public int? ParentId { get; private set; }
        public virtual Category Parent { get; private set; }
        public virtual ICollection<Category> InverseParent { get; private set; }
        public virtual ICollection<Recipe> Recipe { get; private set; }

        public Category()
        {
            InverseParent = new HashSet<Category>();
            Recipe = new HashSet<Recipe>();
        }

        public Category(string name, int? parentId = null) : base(name)
        {
            ParentId = parentId;
        }
    }
}
