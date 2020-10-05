namespace RecipeBook.Core.Entities
{
    public class RecipeIngredient
    {
        public int Id { get; private set; }
        public int RecipeId { get; private set; }
        public int IngredientId { get; private set; }
        public string Amount { get; set; }

        public virtual Ingredient Ingredient { get; set; }
        public virtual Recipe Recipe { get; private set; }
    }
}
