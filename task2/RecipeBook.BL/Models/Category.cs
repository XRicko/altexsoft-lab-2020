namespace RecipeBook.BL.Models
{
    public class Category : ModelBase
    {
        public int? ParentID { get; }
        public string ParentName { get; }

        public Category(string name, int? parentId = null, string parentName = null) : base(name)
        {
            if (parentId != null && parentName != null)
            {
                ParentID = parentId;
                ParentName = parentName;
            }
        }
    }
}
