using RecipeBook.SharedKernel;
using System.Collections.Generic;

namespace RecipeBook.Core.Entities
{
    public class Ingredient : BaseEntity
    {
        public virtual ICollection<RecipeIngredient> RecipeIngredient { get; }

        public Ingredient(string name) : base(name) { }
    }
}
