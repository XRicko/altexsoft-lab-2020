using Moq;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RecipeBook.Core.Tests
{
    public class CategoryControllerTests
    {
        private readonly Mock<IRepository> repoMock;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly CategoryController controller;

        public CategoryControllerTests()
        {
            repoMock = new Mock<IRepository>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            controller = new CategoryController(unitOfWorkMock.Object);

            unitOfWorkMock.SetupGet(x => x.Repository)
                .Returns(repoMock.Object);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldReturnNewCategory()
        {
            // Arrange
            repoMock.Setup(x => x.GetAsync<Category>(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var categoryName = "Drinks";
            var expected = new Category(categoryName);

            // Act
            var actual = await controller.CreateCategoryAsync(categoryName);

            // Assert
            Assert.NotSame(expected, actual);
            Assert.Equal(expected.Name, actual.Name);

            repoMock.Verify(x => x.GetAsync<Category>(It.IsAny<string>()), Times.Once);
            repoMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldReturnExistingCategory()
        {
            // Arrange
            var categoryName = "Soups";
            var expected = new Category(categoryName);

            repoMock.Setup(x => x.GetAsync<Category>(It.IsAny<string>()))
                .ReturnsAsync(expected);
          
            // Act
            var actual = await controller.CreateCategoryAsync(categoryName);

            // Assert
            Assert.Same(expected, actual);

            repoMock.Verify(x => x.GetAsync<Category>(It.IsAny<string>()), Times.Once);
            repoMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldAddParent()
        {
            // Arrange
            var parentName = "Soups";
            var categoryName = "Hot";

            repoMock.Setup(x => x.GetAsync<Category>(It.IsAny<string>()))
                .ReturnsAsync(() => null);
            repoMock.Setup(x => x.GetAllAsync<Category>())
                .ReturnsAsync(new List<Category> { new Category("Drinks") { Id = 3 } });
          
            // Act
            var category = await controller.CreateCategoryAsync(categoryName, parentName);

            // Assert
            Assert.Equal(4, category.ParentId);

            repoMock.Verify(x => x.GetAsync<Category>(It.IsAny<string>()), Times.Exactly(2));
            repoMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldReturnNewSubCategoryToExistingParent()
        {
            // Arrange
            var parentName = "Soups";
            var categoryName = "Hot";

            var parent = new Category(parentName) { Id = 3 };

            repoMock.Setup(x => x.GetAsync<Category>(categoryName))
                .ReturnsAsync(() => null);
            repoMock.Setup(x => x.GetAsync<Category>(parentName))
                .ReturnsAsync(parent);
          
            // Act
            var category = await controller.CreateCategoryAsync(categoryName, parentName);

            // Assert
            Assert.Equal(parent.Id, category.ParentId);

            repoMock.Verify(x => x.GetAsync<Category>(It.IsAny<string>()), Times.Exactly(2));
            repoMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Never);
        }
    }
}
