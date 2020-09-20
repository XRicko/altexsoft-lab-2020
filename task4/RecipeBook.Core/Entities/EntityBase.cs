using System;

namespace RecipeBook.Core.Entities
{
    public class EntityBase
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public EntityBase() { }

        public EntityBase(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"Name cannot be null", nameof(name));

            Name = name;
        }
    }
}
