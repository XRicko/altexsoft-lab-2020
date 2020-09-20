using Microsoft.EntityFrameworkCore;
using RecipeBook.Core.Entities;
using RecipeBook.Core.Repository.Interfaces;

namespace RecipeBook.Core.Repository.Classes
{
    class IngredientRepository : Repository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(DbContext context) : base(context) { }
    }
}
