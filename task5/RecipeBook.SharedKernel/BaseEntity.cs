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
                
                id = value;
            }
        }
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
