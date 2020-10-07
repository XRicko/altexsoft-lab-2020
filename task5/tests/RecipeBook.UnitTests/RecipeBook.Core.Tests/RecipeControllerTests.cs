using Moq;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.Core.Exceptions;
using RecipeBook.SharedKernel;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RecipeBook.Core.Tests
{
    public class RecipeControllerTests
    {
        private readonly Mock<IRepository> repoMock;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly RecipeController controller;

        private readonly string recipeName, description, instruction;
        private readonly Category category;
        private readonly List<RecipeIngredient> recipeIngredients;
        private readonly double duration;

        private readonly Recipe recipe;

        public RecipeControllerTests()
        {
            repoMock = new Mock<IRepository>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            controller = new RecipeController(unitOfWorkMock.Object);

            unitOfWorkMock.SetupGet(x => x.Repository)
                .Returns(repoMock.Object);

            recipeName = "Soup";
            description = "Something delicious";
            instruction = "Bing it :)";
            category = new Category("Soups");
            recipeIngredients = new List<RecipeIngredient>();
            duration = 30;

            recipe = new Recipe(recipeName, category, description, recipeIngredients, instruction, duration);
        }

        [Fact]
        public async Task CreateRecipeAsync_ShouldReturnNewRecipe()
        {
            // Arrange
            repoMock.Setup(x => x.GetAsync<Recipe>(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            var actual = await controller.CreateRecipeAsync(recipeName, category, description, recipeIngredients, instruction, duration);

            // Assert
            Assert.NotSame(recipe, actual);
            Assert.Equal(recipe.Name, actual.Name);

            repoMock.Verify(x => x.GetAsync<Recipe>(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateRecipeAsync_ShouldReturnExistingRecipe()
        {
            // Arrange
            repoMock.Setup(x => x.GetAsync<Recipe>(It.IsAny<string>()))
                .ReturnsAsync(recipe);

            // Act
            var actual = await controller.CreateRecipeAsync(recipeName, category, description, recipeIngredients, instruction, duration);

            // Assert
            Assert.Same(recipe, actual);
            repoMock.Verify(x => x.GetAsync<Recipe>(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddRecipeAsync_ShouldExecute()
        {
            // Arrange
            repoMock.Setup(x => x.GetAsync(It.IsAny<Recipe>()))
                .ReturnsAsync(() => null);
            repoMock.Setup(x => x.GetAllAsync<Recipe>())
                .ReturnsAsync(() => null);

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
            repoMock.Setup(x => x.GetAsync(It.IsAny<Recipe>()))
                .ReturnsAsync(recipe);

            // Assert && Act
            var exception = await Assert.ThrowsAsync<RecipeExistsException>(() => controller.AddRecipeAsync(recipe));
            Assert.Equal(new RecipeExistsException(recipe.Name).Message, exception.Message);

            repoMock.Verify(x => x.AddAsync(It.IsAny<BaseEntity>()), Times.Never);
            unitOfWorkMock.Verify(x => x.SaveAsync(), Times.Never);

        }
    }
}
