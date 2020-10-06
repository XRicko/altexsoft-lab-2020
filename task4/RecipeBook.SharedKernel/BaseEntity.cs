using System;

namespace RecipeBook.SharedKernel
{
    public abstract class BaseEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        protected BaseEntity(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Name cannot be null");

            Name = name;
        }
    }
}
