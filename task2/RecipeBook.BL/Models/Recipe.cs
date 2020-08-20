using System;
using System.Collections.Generic;

namespace RecipeBook.BL.Models
{
    public class Recipe : ModelBase
    {
        public string Description { get; }
        public Category Category { get; }
        public List<RecipeIngredient> Ingredients { get; }
        public List<string> Instruction { get; }
        public double DurationInMinutes { get; }

        public Recipe(string name, Category category, string description, List<RecipeIngredient> ingredients, List<string> instruction, double durationInMinutes) : base(name)
        {
            if (String.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException("Description cannot be null", nameof(description));
            if (durationInMinutes <= 0)
                throw new ArgumentException("Duration cannot be 0 or less", nameof(durationInMinutes));

            Category = category ?? throw new ArgumentNullException("Category cannot be null", nameof(category));
            Description = description;
            Ingredients = ingredients ?? throw new ArgumentNullException("Ingredients cannot be null", nameof(ingredients));
            Instruction = instruction ?? throw new ArgumentNullException("Instruction cannot be null", nameof(instruction));
            DurationInMinutes = durationInMinutes;
        }
    }
}
