using RecipeBook.SharedKernel.Extensions;
using System;

namespace RecipeBook.SharedKernel
{
    public abstract class BaseEntity
    {
        private int id;
        private string name;

        public int Id
        {
            get => id;
            set
            {
                if (value < 1)
                    throw new ArgumentException($"Id should be greater than 0. {id} given instead");
                else
                    id = value;
            }
        }
        public string Name { get => name; set => name = value.StandardizeName(); }

        protected BaseEntity(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Name cannot be null");

            Name = name;
        }

        protected BaseEntity() { }
    }
}
