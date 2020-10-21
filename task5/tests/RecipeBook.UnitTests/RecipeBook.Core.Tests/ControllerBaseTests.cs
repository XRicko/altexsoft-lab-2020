using Moq;
using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RecipeBook.Core.Tests
{
    public class ControllerBaseTests
    {
        private readonly Mock<IRepository> repoMock;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly ControllerBaseForTests controller;

        public ControllerBaseTests()
        {
            repoMock = new Mock<IRepository>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            controller = new ControllerBaseForTests(unitOfWorkMock.Object);

            unitOfWorkMock.SetupGet(x => x.Repository)
              .Returns(repoMock.Object);
        }

        [Fact]
        public async Task GetAllItemsAsync_ShouldReturnList()
        {
            // Arrange
            IEnumerable<Ingredient> expected = GetSampleData();

            repoMock.Setup(x => x.GetAllAsync<Ingredient>())
                            .ReturnsAsync(expected);

            // Act
            var actual = await controller.GetItemsAsync<Ingredient>();

            // Assert
            Assert.Same(expected, actual);
            repoMock.Verify(x => x.GetAllAsync<Ingredient>(), Times.Once);
        }

        [Fact]
        public async Task AddItemAsync_ShouldExecute()
        {
            // Arrange
            var ingredient = new Ingredient("Beef");

            repoMock.Setup(x => x.GetAsync(ingredient))
                .ReturnsAsync(() => null);
            repoMock.Setup(x => x.GetAllAsync<Ingredient>())
                .ReturnsAsync(() => null);

            repoMock.Setup(x => x.AddAsync(ingredient));

            // Act
            await controller.AddItemAsync(ingredient);

            // Assert
            repoMock.Verify(x => x.AddAsync(ingredient), Times.Once);
            repoMock.Verify(x => x.GetAsync(ingredient), Times.Once);
            repoMock.Verify(x => x.GetAllAsync<Ingredient>(), Times.Once);
        }

        [Fact]
        public async Task AddItemAsync_ShouldNotExecute()
        {
            // Arrange
            var ingredientName = "Milk";
            var existingIngredient = new Ingredient(ingredientName);  // Because BaseEntity is abstract and Ingredient is the simplest entity

            var ingredient = new Ingredient(ingredientName);

            repoMock.Setup(x => x.GetAsync(ingredient))
                .ReturnsAsync(existingIngredient);

            // Act
            await controller.AddItemAsync(ingredient);

            // Assert
            repoMock.Verify(x => x.AddAsync(ingredient), Times.Never);
            repoMock.Verify(x => x.GetAllAsync<Ingredient>(), Times.Never);
        }

        public IEnumerable<Ingredient> GetSampleData()
        {
            var output = new List<Ingredient>
            {
                new Ingredient("Butter"),
                new Ingredient("Bread"),
                new Ingredient("Salami"),
            };

            return output;
        }
    }
}
