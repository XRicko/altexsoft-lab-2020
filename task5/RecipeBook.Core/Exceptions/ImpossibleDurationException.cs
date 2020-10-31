using System;

namespace RecipeBook.Core.Exceptions
{
    class ImpossibleDurationException : Exception
    {
        public ImpossibleDurationException() { }

        public ImpossibleDurationException(string message)
            : base(message) { }

        public ImpossibleDurationException(string message, Exception inner)
            : base(message, inner) { }

        public ImpossibleDurationException(double? duration)
            : this($"Duration should be greater than 0. {duration} given instead") { }
    }
}
