using RecipeBook.Core.Exceptions;
using RecipeBook.SharedKernel;
using System;
using System.Collections.Generic;

namespace RecipeBook.Core.Entities
{
    public class Recipe : BaseEntity
    {
        private string description;
        private double? durationInMinutes;
        private string instruction;
        private Category category;
        private ICollection<RecipeIngredient> recipeIngredients;

        public string Description
        {
            get => description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(description), "Description cannot be null");
                
                description = value;
            }
        }
        public string Instruction
        {
            get => instruction;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(instruction), "Instruction cannot be null");
                
                instruction = value;
            }
        }
        public double? DurationInMinutes
        {
            get => durationInMinutes.GetValueOrDefault();
            set
            {
                if (value <= 0)
                    throw new ImpossibleDurationException(value);
                
                durationInMinutes = value;
            }
        }

        public int CategoryId { get; private set; }
        public virtual Category Category
        {
            get => category;
            set => category = value ?? throw new ArgumentNullException(nameof(category), "Category cannot be null");
        }

        public virtual ICollection<RecipeIngredient> RecipeIngredients
        {
            get => recipeIngredients;
            set => recipeIngredients = value ?? throw new ArgumentNullException(nameof(recipeIngredients), "RecipeIngredients cannot be null");
        }

        public Recipe(string name) : base(name) { }

        public Recipe(string name, Category category, string description, List<RecipeIngredient> recipeIngredients, string instruction, double durationInMinutes) : this(name)
        {
            Category = category;
            Description = description;
            RecipeIngredients = recipeIngredients;
            Instruction = instruction;
            DurationInMinutes = durationInMinutes;
        }

        public Recipe() : base() { }
    }
}
