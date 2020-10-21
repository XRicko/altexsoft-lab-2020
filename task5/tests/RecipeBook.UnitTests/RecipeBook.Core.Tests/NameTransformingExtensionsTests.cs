using RecipeBook.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace RecipeBook.Core.Tests
{
    public class NameTransformingExtensionsTests
    {
        [Fact]
        public void StandardizeName_ShouldReturnStringToTitleCase()
        {
            // Assert && Act
            Assert.Equal("Something Interesting", " somEthing interesting   ".StandardizeName());
        }

        [Fact]
        public void StandardizeName_ShouldReturnNull()
        {
            Assert.Null(" ".StandardizeName());
        }
    }
}
