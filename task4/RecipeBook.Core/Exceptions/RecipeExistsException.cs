using System;

namespace RecipeBook.Core.Exceptions
{
    class RecipeExistsException : Exception
    {
        public RecipeExistsException(string recipeName)
            : base($"{recipeName} already exists") { }
    }
}
