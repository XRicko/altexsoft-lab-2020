using Microsoft.EntityFrameworkCore;
using RecipeBook.Core.Entities;
using RecipeBook.Core.Repository.Interfaces;

namespace RecipeBook.Core.Repository.Classes
{
    class RecipeRepository : Repository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(DbContext context) : base(context) { }
    }
}
