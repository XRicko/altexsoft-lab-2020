using Moq;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel;
using RecipeBook.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RecipeBook.Core.Tests
{
    public class RecipeControllerTests
    {
        [Fact]
        public async Task CreateRecipeAsync_ShouldReturnNewRecipe()
        {
            // Arrange
            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.GetAsync<Recipe>(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Recipe>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new RecipeController(unitOfWorkMock.Object);

            string recipeName = "Soup", description = "Something delicious", instruction = "Bing it :)";
            var category = new Category("Soups");
            var recipeIngredients = new List<RecipeIngredient>();
            double duration = 30;

            var expected = new Recipe(recipeName, category, description, recipeIngredients, instruction, duration);

            // Act
            var actual = await controller.CreateRecipeAsync(recipeName, category, description, recipeIngredients, instruction, duration);

            // Assert
            Assert.NotSame(expected, actual);
            Assert.Equal(expected.Name, actual.Name);
        }

        [Fact]
        public async Task CreateRecipeAsync_ShouldReturnExistingRecipe()
        {
            // Arrange
            string recipeName = "Soup", description = "Something delicious", instruction = "Bing it :)";
            var category = new Category("Soups");
            var recipeIngredients = new List<RecipeIngredient>();
            double duration = 30;

            var expected = new Recipe(recipeName, category, description, recipeIngredients, instruction, duration);

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.GetAsync<Recipe>(It.IsAny<string>()))
                .ReturnsAsync(expected);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new RecipeController(unitOfWorkMock.Object);

            // Act
            var actual = await controller.CreateRecipeAsync(recipeName, category, description, recipeIngredients, instruction, duration);

            // Assert
            Assert.Same(expected, actual);
        }

        [Fact]
        public async Task AddRecipeAsync_ShouldExecute()
        {
            // Arrange
            var repoMock = new Mock<IRepository>();

            repoMock.Setup(x => x.GetAsync(It.IsAny<Recipe>()))
                .ReturnsAsync(It.IsAny<Recipe>());
            repoMock.Setup(x => x.GetAllAsync<Recipe>())
                .ReturnsAsync(It.IsAny<IEnumerable<Recipe>>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new RecipeController(unitOfWorkMock.Object);

            string recipeName = "Soup", description = "Something delicious", instruction = "Bing it :)";
            var category = new Category("Soups");
            var recipeIngredients = new List<RecipeIngredient>();
            double duration = 30;

            var recipe = new Recipe(recipeName, category, description, recipeIngredients, instruction, duration);

            // Act
            await controller.AddRecipeAsync(recipe);

            // Assert
            repoMock.Verify(x => x.AddAsync(It.IsAny<BaseEntity>()), Times.Exactly(recipeIngredients.Count + 2));
            unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task AddRecipeAsync_ShouldThrowException()
        {
            // Arrange
            string recipeName = "Soup", description = "Something delicious", instruction = "Bing it :)";
            var category = new Category("Soups");
            var recipeIngredients = new List<RecipeIngredient>();
            double duration = 30;

            var recipe = new Recipe(recipeName, category, description, recipeIngredients, instruction, duration);

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.GetAsync(It.IsAny<Recipe>()))
                .ReturnsAsync(recipe);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new RecipeController(unitOfWorkMock.Object);

            // Assert && Act
            var exception =  await Assert.ThrowsAsync<ArgumentException>(() => controller.AddRecipeAsync(recipe));
            Assert.Equal(new ArgumentException("This recipe already exists", nameof(recipe)).Message, exception.Message);
        }
    }
}
