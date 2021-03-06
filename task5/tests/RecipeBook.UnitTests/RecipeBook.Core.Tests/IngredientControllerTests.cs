using Moq;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Extensions;
using RecipeBook.SharedKernel.Interfaces;
using System.Threading.Tasks;
using Xunit;

namespace RecipeBook.Core.Tests
{
    public class IngredientControllerTests
    {
        private readonly Mock<IRepository> repoMock;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly IngredientController controller;

        private readonly string ingredientName;
        private readonly Ingredient ingredient;

        public IngredientControllerTests()
        {
            repoMock = new Mock<IRepository>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            controller = new IngredientController(unitOfWorkMock.Object);

            unitOfWorkMock.SetupGet(x => x.Repository)
                .Returns(repoMock.Object);

            ingredientName = "Cheese";
            ingredient = new Ingredient(ingredientName);
        }

        [Fact]
        public async Task GetOrCreateIngredientAsync_ShouldReturnNewIngredient()
        {
            // Arrange
            repoMock.Setup(x => x.GetAsync<Ingredient>(ingredientName))
                .ReturnsAsync(() => null);

            // Act
            Ingredient actual = await controller.GetOrCreateIngredientAsync(ingredientName);

            // Arrange
            Assert.Equal(ingredient.Name.StandardizeName(), actual.Name);
            repoMock.Verify(x => x.GetAsync<Ingredient>(ingredientName), Times.Once);
        }

        [Fact]
        public async Task GetOrCreateIngredientAsync_ShouldReturnExistingIngredient()
        {
            // Arrange
            repoMock.Setup(x => x.GetAsync<Ingredient>(ingredientName))
                .ReturnsAsync(ingredient);

            // Act
            Ingredient actual = await controller.GetOrCreateIngredientAsync(ingredientName);

            // Assert
            Assert.Same(ingredient, actual);
            repoMock.Verify(x => x.GetAsync<Ingredient>(ingredientName), Times.Once);
        }
    }
}
