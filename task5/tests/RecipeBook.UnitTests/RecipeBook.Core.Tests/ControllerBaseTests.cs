using Moq;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RecipeBook.Core.Tests
{
    public class ControllerBaseTests
    {
        private readonly Mock<IRepository> repoMock;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly IngredientController controller;

        public ControllerBaseTests()
        {
            repoMock = new Mock<IRepository>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            controller = new IngredientController(unitOfWorkMock.Object);   // Because ControllerBase is abstract and IngredientController is the simplest controller

            unitOfWorkMock.SetupGet(x => x.Repository)
              .Returns(repoMock.Object);
        }

        [Fact]
        public async Task GetAllItemsAsync_ShouldReturnList()
        {
            // Arrange
            repoMock.Setup(x => x.GetAllAsync<Ingredient>())
                .ReturnsAsync(GetSampleData());

            var expected = GetSampleData();

            // Act
            var actual = await controller.GetItemsAsync<Ingredient>();

            // Assert
            Assert.True(actual != null);
            Assert.Equal(expected.Count(), actual.Count());
            Assert.Equal(expected.FirstOrDefault().Name, actual.FirstOrDefault().Name);

            repoMock.Verify(x => x.GetAllAsync<Ingredient>(), Times.Once);
        }

        [Fact]
        public async Task AddItemAsync_ShouldExecute()
        {
            // Arrange
            repoMock.Setup(x => x.GetAsync(It.IsAny<Ingredient>()))
                .ReturnsAsync(() => null);
            repoMock.Setup(x => x.GetAllAsync<Ingredient>())
                .ReturnsAsync(() => null);

            var ingredient = new Ingredient("Beef");

            // Act
            await controller.AddItemAsync(ingredient);

            // Assert
            repoMock.Verify(x => x.AddAsync(ingredient), Times.Once);
        }

        [Fact]
        public async Task AddItemAsync_ShouldNotExecute()
        {
            // Arrange
            var ingredientName = "Milk";
            var existingIngredient = new Ingredient(ingredientName);  // Because BaseEntity is abstract and Ingredient is the simplest entity

            repoMock.Setup(x => x.GetAsync(It.IsAny<Ingredient>()))
                .ReturnsAsync(existingIngredient);
        
            var ingredient = new Ingredient(ingredientName);

            // Act
            await controller.AddItemAsync(ingredient);

            // Assert
            repoMock.Verify(x => x.AddAsync(ingredient), Times.Never);
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
