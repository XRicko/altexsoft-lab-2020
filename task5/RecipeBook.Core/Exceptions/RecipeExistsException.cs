using System;

namespace RecipeBook.Core.Exceptions
{
    public class RecipeExistsException : Exception
    {
        public RecipeExistsException(string recipeName)
            : base($"{recipeName} already exists") { }
    }
}
