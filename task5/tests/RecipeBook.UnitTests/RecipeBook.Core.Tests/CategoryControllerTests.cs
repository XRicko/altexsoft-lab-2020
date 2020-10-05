using Moq;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Interfaces;
using System.Threading.Tasks;
using Xunit;

namespace RecipeBook.Core.Tests
{
    public class CategoryControllerTests
    {
        [Fact]
        public async Task CreateCategoryAsync_ShouldReturnNewCategory()
        {
            // Arrange
            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.GetAsync<Category>(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Category>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new CategoryController(unitOfWorkMock.Object);

            var categoryName = "Drinks";
            var expected = new Category(categoryName);

            // Act
            var actual = await controller.CreateCategoryAsync(categoryName);

            // Assert
            Assert.NotSame(expected, actual);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldReturnExistingCategory()
        {
            // Arrange
            var categoryName = "Soups";
            var expected = new Category(categoryName);

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.GetAsync<Category>(It.IsAny<string>()))
                .ReturnsAsync(expected);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new CategoryController(unitOfWorkMock.Object);

            // Act
            var actual = await controller.CreateCategoryAsync(categoryName);

            // Assert
            Assert.Same(expected, actual);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldAddParent()
        {
            // Arrange
            var parentName = "Soups";
            var categoryName = "Hot";

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.GetAsync<Category>(It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<Category>());

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new CategoryController(unitOfWorkMock.Object);

            // Act
            var category = await controller.CreateCategoryAsync(categoryName, parentName);

            // Assert
            repoMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldReturnNewSubCategoryToExistingParent()
        {
            // Arrange
            var parentName = "Soups";
            var categoryName = "Hot";

            var parent = new Category(parentName) { Id = 3 };

            var repoMock = new Mock<IRepository>();
            repoMock.Setup(x => x.GetAsync<Category>(parentName))
                .ReturnsAsync(parent);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.Repository)
                .Returns(repoMock.Object);

            var controller = new CategoryController(unitOfWorkMock.Object);

            // Act
            var category = await controller.CreateCategoryAsync(categoryName, parentName);

            // Assert
            Assert.Equal(parent.Id, category.ParentId);
        }
    }
}
