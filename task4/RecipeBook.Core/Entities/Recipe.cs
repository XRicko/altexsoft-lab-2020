using System;
using System.Collections.Generic;

namespace RecipeBook.Core.Entities
{
    public partial class Recipe : EntityBase
    {
        public string Description { get; private set; }
        public int CategoryId { get; private set; }
        public string Instruction { get; private set; }
        public double? DurationInMinutes { get; private set; }

        public virtual Category Category { get; private set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredient { get; private set; }

        public Recipe()
        {
            RecipeIngredient = new HashSet<RecipeIngredient>();
        }

        public Recipe(string name, Category category, string description, List<RecipeIngredient> recipeIngredients, string instruction, double durationInMinutes) : base(name)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("Description cannot be null", nameof(description));
            if (durationInMinutes <= 0)
                throw new ArgumentException("Duration cannot be 0 or less", nameof(durationInMinutes));
            if (string.IsNullOrWhiteSpace(instruction))
                throw new ArgumentNullException("Instruction cannot be null", nameof(instruction));

            Category = category ?? throw new ArgumentNullException("Category cannot be null", nameof(category));
            Description = description;
            RecipeIngredient = recipeIngredients ?? throw new ArgumentNullException("Ingredients cannot be null", nameof(recipeIngredients));
            Instruction = instruction;
            DurationInMinutes = durationInMinutes;
        }
    }
}
