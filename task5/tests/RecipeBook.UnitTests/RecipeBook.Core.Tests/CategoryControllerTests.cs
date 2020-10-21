using Moq;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.Core.Extensions;
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
            var categoryName = "Drinks";

            repoMock.Setup(x => x.GetAsync<Category>(categoryName))
                .ReturnsAsync(() => null);

            // Act
            var actual = await controller.CreateCategoryAsync(categoryName);

            // Assert
            Assert.Equal(categoryName, actual.Name);

            repoMock.Verify(x => x.GetAsync<Category>(categoryName), Times.Once);
            repoMock.Verify(x => x.AddAsync(It.Is<Category>(x => x.Name == categoryName)), Times.Never);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldReturnExistingCategory()
        {
            // Arrange
            var categoryName = "Soups";
            var expected = new Category(categoryName);

            repoMock.Setup(x => x.GetAsync<Category>(categoryName))
                .ReturnsAsync(expected);

            // Act
            var actual = await controller.CreateCategoryAsync(categoryName);

            // Assert
            Assert.Same(expected, actual);
            repoMock.Verify(x => x.GetAsync<Category>(categoryName), Times.Once);
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldAddParent()
        {
            // Arrange
            var parentName = "Soups";
            var categoryName = "Hot";

            var standardizedCategoryName = categoryName.StandardizeName();
            var standardizedParentName = parentName.StandardizeName();

            repoMock.Setup(x => x.GetAsync<Category>(standardizedCategoryName))
                .ReturnsAsync(() => null);
            repoMock.Setup(x => x.GetAsync<Category>(standardizedParentName))
                .ReturnsAsync(() => null);

            repoMock.Setup(x => x.GetAllAsync<Category>())
                .ReturnsAsync(new List<Category> { new Category("Drinks") { Id = 3 } });

            repoMock.Setup(x => x.AddAsync(It.Is<Category>(c => c.Name == standardizedParentName)));

            var expectedName = (standardizedCategoryName + " " + standardizedParentName).RemoveDublicates();

            // Act
            var category = await controller.CreateCategoryAsync(categoryName, parentName);

            // Assert
            Assert.Equal(4, category.ParentId);
            Assert.Equal(expectedName, category.Name);

            repoMock.Verify(x => x.GetAsync<Category>(It.Is<string>(s => s == standardizedCategoryName ||
                                                                         s == standardizedParentName)),
                                                                         Times.Exactly(2));
        }

        [Fact]
        public async Task CreateCategoryAsync_ShouldReturnNewSubCategoryToExistingParent()
        {
            // Arrange
            var parentName = "Soups";
            var categoryName = "Hot";

            var standardizedCategoryName = categoryName.StandardizeName();
            var standardizedParentName = parentName.StandardizeName();

            var parent = new Category(standardizedParentName) { Id = 3 };

            repoMock.Setup(x => x.GetAsync<Category>(standardizedCategoryName))
                .ReturnsAsync(() => null);
            repoMock.Setup(x => x.GetAsync<Category>(standardizedParentName))
                .ReturnsAsync(parent);

            var expectedName = (standardizedCategoryName + " " + standardizedParentName).RemoveDublicates();

            // Act
            var category = await controller.CreateCategoryAsync(categoryName, parentName);

            // Assert
            Assert.Equal(parent.Id, category.ParentId);
            Assert.Equal(expectedName, category.Name);

            repoMock.Verify(x => x.GetAsync<Category>(It.Is<string>(s => s == standardizedCategoryName ||
                                                                         s == standardizedParentName)), Times.Exactly(2));
        }
    }
}
