using RecipeBook.Core.Extensions;
using System.Collections.Generic;
using Xunit;

namespace RecipeBook.Core.Tests
{
    public class StringTransformingExtensions
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

        [Fact]
        public void GetWords_ShouldReturnListOfWords()
        {
            // Arrange
            var words = new List<string> { "word1", "word2", "word3", "word7", "word8", "word10" };
            var str = "word1, word2,     word3, word7, word8,   word10";

            // Assetr && Act
            Assert.Equal(words, str.GetWords());
        }
    }
}
