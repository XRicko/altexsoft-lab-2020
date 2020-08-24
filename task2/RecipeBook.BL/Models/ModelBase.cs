using System;

namespace RecipeBook.BL.Models
{
    public abstract class ModelBase
    {
        public int Id { get; set; }
        public string Name { get; protected set; }

        public ModelBase(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException($"Name cannot be null", nameof(name));

            Name = name;
        }
    }
}
