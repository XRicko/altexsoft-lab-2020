using RecipeBook.Core.Entities;
using System;

namespace RecipeBook.Core.Exceptions
{
    class RecipeExistsException : Exception
    {
        public RecipeExistsException() { }

        public RecipeExistsException(string message)
            : base(message) { }

        public RecipeExistsException(string message, Exception inner)
            : base(message, inner) { }

        public RecipeExistsException(Recipe recipe)
            : this($"{recipe.Name} already exists") { }
    }
}
