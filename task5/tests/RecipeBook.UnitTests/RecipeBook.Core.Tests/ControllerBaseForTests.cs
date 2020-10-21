using RecipeBook.Core.Controllers;
using RecipeBook.SharedKernel.Interfaces;

namespace RecipeBook.Core.Tests
{
    class ControllerBaseForTests : ControllerBase
    {
        private IUnitOfWork unitOfWork;

        public ControllerBaseForTests(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
