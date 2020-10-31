﻿using Moq;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.SharedKernel.Extensions;
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
        public async Task GetOrCreateCategoryAsync_ShouldAddParent()
        {
            // Arrange
            var parentName = "Soups";
            var categoryName = "Hot";

            string standardizedCategoryName = categoryName.StandardizeName();
            string standardizedParentName = parentName.StandardizeName();
            string expectedName = (standardizedCategoryName + " " + standardizedParentName).RemoveDublicates();

            repoMock.Setup(x => x.GetAsync<Category>(expectedName))
                .ReturnsAsync(() => null);
            repoMock.Setup(x => x.GetAsync<Category>(standardizedParentName))
                .ReturnsAsync(() => null);

            repoMock.Setup(x => x.GetAllAsync<Category>())
                .ReturnsAsync(new List<Category> { new Category("Drinks") { Id = 3 } });

            repoMock.Setup(x => x.AddAsync(It.Is<Category>(c => c.Name == standardizedParentName)));

            // Act
            Category category = await controller.GetOrCreateCategoryAsync(categoryName, parentName);

            // Assert
            Assert.Equal(4, category.ParentId);
            Assert.Equal(expectedName, category.Name);

            repoMock.Verify(x => x.GetAsync<Category>(It.Is<string>(s => s == expectedName ||
                                                                         s == standardizedParentName)),
                                                                         Times.Exactly(2));
        }

        [Fact]
        public async Task GetOrCreateCategoryAsync_ShouldReturnNewSubCategoryToExistingParent()
        {
            // Arrange
            var parentName = "Soups";
            var categoryName = "Hot";

            string standardizedCategoryName = categoryName.StandardizeName();
            string standardizedParentName = parentName.StandardizeName();
            string expectedName = (standardizedCategoryName + " " + standardizedParentName).RemoveDublicates();

            var parent = new Category(standardizedParentName) { Id = 3 };

            repoMock.Setup(x => x.GetAsync<Category>(expectedName))
                .ReturnsAsync(() => null);
            repoMock.Setup(x => x.GetAsync<Category>(standardizedParentName))
                .ReturnsAsync(parent);

            // Act
            Category category = await controller.GetOrCreateCategoryAsync(categoryName, parentName);

            // Assert
            Assert.Equal(parent.Id, category.ParentId);
            Assert.Equal(expectedName, category.Name);

            repoMock.Verify(x => x.GetAsync<Category>(It.Is<string>(s => s == expectedName ||
                                                                         s == standardizedParentName)), Times.Exactly(2));
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldGiveParentToCategory()
        {
            // Arrange
            var subcategoryName = "Hot";
            var parentName = "Soups";
            var categoryName = subcategoryName + " " + parentName;

            var categoryToUpdate = new Category(subcategoryName);

            repoMock.Setup(x => x.Update(It.Is<Category>(c => c.Name == categoryName)));

            // Act
            await controller.UpdateCategoryAsync(categoryToUpdate, parentName);

            // Assert
            Assert.Equal(categoryName, categoryToUpdate.Name);
            Assert.Equal(parentName, categoryToUpdate.Parent.Name);

            repoMock.Verify(x => x.Update(It.Is<Category>(c => c.Name == categoryName)), Times.Once);
        }
    }
}
