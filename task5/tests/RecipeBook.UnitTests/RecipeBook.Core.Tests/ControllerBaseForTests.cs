using RecipeBook.Core.Controllers;
using RecipeBook.SharedKernel.Interfaces;

namespace RecipeBook.Core.Tests
{
    class ControllerBaseForTests : ControllerBase
    {
        public ControllerBaseForTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}