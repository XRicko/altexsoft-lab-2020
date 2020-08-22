namespace RecipeBook.BL.Models
{
    public class Category : ModelBase
    {
        public int? ParentId { get; }
        public string ParentName { get; }

        public Category(string name, int? parentId = null, string parentName = null) : base(name)
        {
            if (parentId != null && parentName != null)
            {
                ParentId = parentId;
                ParentName = parentName;
            }
        }
    }
}
