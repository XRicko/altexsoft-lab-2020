using Moq;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Interfaces;
using System.Threading.Tasks;
using Xunit;

namespace RecipeBook.Core.Tests
{
    public class IngredientControllerTests
    {
        [Fact]
        public async Task CreateIngredientAsync_ShouldReturnNewIngredient()
        {
            // Arrange
            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.GetAsync<Ingredient>(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Ingredient>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new IngredientController(unitOfWorkMock.Object);

            var ingredientName = "Pork";
            var expected = new Ingredient(ingredientName);

            // Act
            var actual = await controller.CreateIngredientAsync(ingredientName);

            // Arrange
            Assert.NotSame(expected, actual);
            Assert.Equal(expected.Name, actual.Name);
        }

        [Fact]
        public async Task CreateIngredientAsync_ShouldReturnExistingIngredient()
        {
            // Arrange
            var ingredientName = "Cheese";
            var expected = new Ingredient(ingredientName);

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.GetAsync<Ingredient>(It.IsAny<string>()))
                .ReturnsAsync(expected);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new IngredientController(unitOfWorkMock.Object);

            // Act
            var actual = await controller.CreateIngredientAsync(ingredientName);

            // Assert
            Assert.Same(expected, actual);
        }
    }
}
