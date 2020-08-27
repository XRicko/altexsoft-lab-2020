using RecipeBook.BL.Controllers;
using RecipeBook.BL.Models;
using RecipeBook.BL.Repository.Interfaces;

namespace RecipeBook.BL.Repository.Classes
{
    class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDataManager manager) : base(manager) { }
    }
}
