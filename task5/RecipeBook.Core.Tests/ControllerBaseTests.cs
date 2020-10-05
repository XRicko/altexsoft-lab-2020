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
        [Fact]
        public async Task GetAllItemsAsync_ShouldReturnList()
        {
            // Arrange
            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.GetAllAsync<Ingredient>())
                .ReturnsAsync(GetSampleData());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new IngredientController(unitOfWorkMock.Object);   // Because ControllerBase is abstract and IngredientController is the simplest controller

            var expected = GetSampleData();

            // Act
            var actual = await controller.GetItemsAsync<Ingredient>();

            // Assert
            Assert.True(actual != null);
            Assert.Equal(expected.Count(), actual.Count());
            Assert.Equal(expected.FirstOrDefault().Name, actual.FirstOrDefault().Name);
        }

        [Fact]
        public async Task AddItemAsync_ShouldExecute()
        {
            // Arrange
            var repoMock = new Mock<IRepository>();

            repoMock.Setup(x => x.GetAsync(It.IsAny<Ingredient>()))
                .ReturnsAsync(It.IsAny<Ingredient>());
            repoMock.Setup(x => x.GetAllAsync<Ingredient>())
                .ReturnsAsync(GetSampleData());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new IngredientController(unitOfWorkMock.Object);
            var newIngredient = new Ingredient("Beef");

            // Act
            await controller.AddItemAsync(newIngredient);

            // Assert
            repoMock.Verify(x => x.AddAsync(newIngredient), Times.Once);
        }

        [Fact]
        public async Task AddItemAsync_ShouldNotExecute()
        {
            // Arrange
            var ingredientName = "Milk";
            var expected = new Ingredient(ingredientName);  // Because BaseEntity is abstract and Ingredient is the simplest entity

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.GetAsync(It.IsAny<Ingredient>()))
                .ReturnsAsync(expected);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new IngredientController(unitOfWorkMock.Object);
            var newIngredient = new Ingredient(ingredientName);

            // Act
            await controller.AddItemAsync(newIngredient);

            // Assert
            repoMock.Verify(x => x.AddAsync(newIngredient), Times.Never);
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
