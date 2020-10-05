using RecipeBook.SharedKernel;
using System;
using System.Collections.Generic;

namespace RecipeBook.Core.Entities
{
    public class Recipe : BaseEntity
    {
        public string Description { get; private set; }
        public int CategoryId { get; private set; }
        public string Instruction { get; private set; }
        public double? DurationInMinutes { get; private set; }

        public virtual Category Category { get; private set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; private set; }

        public Recipe(string name) : base(name) { }

        public Recipe(string name, Category category, string description, List<RecipeIngredient> recipeIngredients, string instruction, double durationInMinutes) : base(name)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException(nameof(description), "Description cannot be null");
            if (durationInMinutes <= 0)
                throw new ArgumentException("Duration cannot be 0 or less", nameof(durationInMinutes));
            if (string.IsNullOrWhiteSpace(instruction))
                throw new ArgumentNullException(nameof(instruction), "Instruction cannot be null");

            Category = category ?? throw new ArgumentNullException(nameof(category), "Category cannot be null");
            Description = description;
            RecipeIngredients = recipeIngredients ?? throw new ArgumentNullException(nameof(recipeIngredients), "Ingredients cannot be null");
            Instruction = instruction;
            DurationInMinutes = durationInMinutes;
        }
    }
}
