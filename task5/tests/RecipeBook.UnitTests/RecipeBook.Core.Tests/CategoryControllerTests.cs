using Moq;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Extensions;
using RecipeBook.SharedKernel.Interfaces;
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
        public async Task GetOrCreateCategoryAsync_ShouldReturnNewCategory()
        {
            // Arrange
            var categoryName = "Drinks";

            repoMock.Setup(x => x.GetAsync<Category>(categoryName))
                .ReturnsAsync(() => null);

            // Act
            var actual = await controller.GetOrCreateCategoryAsync(categoryName);

            // Assert
            Assert.Equal(categoryName, actual.Name);

            repoMock.Verify(x => x.GetAsync<Category>(categoryName), Times.Once);
            repoMock.Verify(x => x.AddAsync(It.Is<Category>(x => x.Name == categoryName)), Times.Never);
        }

        [Fact]
        public async Task GetOrCreateCategoryAsync_ShouldReturnExistingCategory()
        {
            // Arrange
            var categoryName = "Hot Soups";

            var expected = new Category(categoryName);

            repoMock.Setup(x => x.GetAsync<Category>(categoryName))
                .ReturnsAsync(expected);

            // Act
            var actual = await controller.GetOrCreateCategoryAsync(categoryName);

            // Assert
            Assert.Same(expected, actual);
            repoMock.Verify(x => x.GetAsync<Category>(categoryName), Times.Once);
        }

        [Fact]
        public async Task GetOrCreateCategoryAsync_ShouldReturnNewSubcategoryToParent()
        {
            // Arrange
            var parentId = 3;
            var parentName = "Soups";
            var categoryName = "Hot Soups";

            string standardizedCategoryName = categoryName.StandardizeName();
            string standardizedParentName = parentName.StandardizeName();

            var parent = new Category(standardizedParentName) { Id = parentId };


            repoMock.Setup(x => x.GetAsync<Category>(categoryName))
                .ReturnsAsync(() => null);
            repoMock.Setup(x => x.GetAsync<Category>(parentId))
                .ReturnsAsync(parent);

            // Act
            Category category = await controller.GetOrCreateCategoryAsync(categoryName, parent.Id);

            // Assert
            Assert.Equal(parent.Id, category.ParentId);
            repoMock.Verify();
        }
    }
}
