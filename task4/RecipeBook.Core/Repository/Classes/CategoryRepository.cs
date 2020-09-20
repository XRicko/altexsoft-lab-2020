using Microsoft.EntityFrameworkCore;
using RecipeBook.Core.Entities;
using RecipeBook.Core.Repository.Interfaces;

namespace RecipeBook.Core.Repository.Classes
{
    class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext context) : base(context) { }
    }
}
