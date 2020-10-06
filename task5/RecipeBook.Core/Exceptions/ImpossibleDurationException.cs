using System;

namespace RecipeBook.Core.Exceptions
{
    class ImpossibleDurationException : Exception
    {
        public ImpossibleDurationException(double duration)
            : base($"Duration should be greater than 0. {duration} given instead") { }
    }
}
