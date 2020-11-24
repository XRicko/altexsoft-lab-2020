using RecipeBook.SharedKernel.Extensions;
using System;

namespace RecipeBook.SharedKernel
{
    public abstract class BaseEntity
    {
        private string name;

        public int Id { get; private set; }
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(name), "Name cannot be null");

                name = value.StandardizeName();
            }
        }

        protected BaseEntity(string name)
        {
            Name = name;
        }

        protected BaseEntity() { }
    }
}
