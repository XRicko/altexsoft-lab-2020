using RecipeBook.SharedKernel;
using System.Collections.Generic;

namespace RecipeBook.Core.Entities
{
    public partial class Ingredient : BaseEntity
    {
        public virtual ICollection<RecipeIngredient> RecipeIngredient { get; }

        public Ingredient()
        {
            RecipeIngredient = new HashSet<RecipeIngredient>();
        }

        public Ingredient(string name) : base(name) { }
    }
}
