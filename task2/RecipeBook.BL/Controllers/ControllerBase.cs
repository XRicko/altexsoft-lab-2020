using RecipeBook.BL.Repository.Classes;

namespace RecipeBook.BL.Controllers
{
    public abstract class ControllerBase
    {
        internal UnitOfWork unitOfWork = new UnitOfWork();
    }
}
