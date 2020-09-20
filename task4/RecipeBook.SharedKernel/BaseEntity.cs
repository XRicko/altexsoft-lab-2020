using System;

namespace RecipeBook.SharedKernel
{
    public class BaseEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public BaseEntity() { }

        public BaseEntity(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"Name cannot be null", nameof(name));

            Name = name;
        }
    }
}
