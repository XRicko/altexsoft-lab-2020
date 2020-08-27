namespace RecipeBook.BL.Models
{
    public class Category : ModelBase
    {
        public int? ParentId { get; }

        public Category(string name, int? parentId = null) : base(name)
        {
            if (parentId != null)
                ParentId = parentId;
        }
    }
}
