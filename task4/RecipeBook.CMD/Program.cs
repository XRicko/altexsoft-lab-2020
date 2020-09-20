using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.Core.Repository.Classes;
using RecipeBook.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace RecipeBook.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new RecipeBookContext();
            var unitOfWork = new UnitOfWork(db);

            var categoryController = new CategoryController(unitOfWork);
            var ingredientController = new IngredientController(unitOfWork);
            var recipeController = new RecipeController(unitOfWork);

            while (true)
            {
                Console.WriteLine("\nChoose what you want to do");

                Console.WriteLine("A - add recipe");
                Console.WriteLine("E - browse through categories and recipes");
                Console.WriteLine("F - see all ingredients");
                Console.WriteLine("Q - exit");

                var key = Console.ReadKey();
                Console.WriteLine();
                Console.Clear();

                switch (key.Key)
                {
                    case ConsoleKey.A:
                        var recipe = EnterRecipe(recipeController, categoryController, ingredientController);
                        recipeController.AddRecipe(recipe);

                        break;
                    case ConsoleKey.F:
                        foreach (var item in ingredientController.GetIngredients())
                        {
                            Console.WriteLine(item.Name);
                        }

                        break;
                    case ConsoleKey.E:
                        var categories = categoryController.GetCategories(null);
                        var recipies = recipeController.GetRecipesInCategory(null);

                        while (true)
                        {
                            if (categories.Count == 0 && recipies.Count == 0)
                            {
                                Console.WriteLine("No elements here");
                                break;
                            }

                            ShowItems(categories);
                            ShowItems(recipies);

                            var name = Request("Enter name to go next or to see full recipe: ");
                            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());

                            var rec = recipies.SingleOrDefault(r => r.Name == name);

                            if (rec != null)
                            {
                                ShowRecipe(rec);
                                break;
                            }
                            else
                                recipies = recipeController.GetRecipesInCategory(name);

                            var category = categories.SingleOrDefault(c => c.Name == name);

                            if (category == null)
                                Console.WriteLine("Invalid input\n");
                            else
                                categories = categoryController.GetCategories(category.Id);
                        }
                        break;
                    case ConsoleKey.Q:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
        }

        static Recipe EnterRecipe(RecipeController recipeController, CategoryController categoryController, IngredientController ingredientController)
        {
            var name = Request("Enter name of recipe: ");
            var categoryName = Request("Enter category (starting with subcats): ");

            Category category;

            while (true)
            {
                var key = Request("Is it a subcategory? (y/n): ");

                if (key == "y")
                {
                    var parentName = Request("Enter parent category: ");
                    category = categoryController.CreateCategory(categoryName, parentName);

                    break;
                }
                if (key == "n")
                {
                    category = categoryController.CreateCategory(categoryName);
                    break;
                }
                else
                    Console.Write("Invalid input");
            }

            var description = Request("Enter a brief description: ");
            var ingredients = Request("Enter ingredients (comma-separated): ");

            var recipeIngredients = new List<RecipeIngredient>();

            foreach (var item in GetWords(ingredients))
            {
                var recipeIngredient = new RecipeIngredient
                {
                    Ingredient = ingredientController.CreateIngredient(item)
                };

                recipeIngredients.Add(recipeIngredient);
            }

            foreach (var item in recipeIngredients)
            {
                var amount = Request($"Enter amount of {item.Ingredient.Name} (e.g., 1 tablespoon, 200 grams): ");
                item.Amount = amount;
            }

            var instruction = Request("Enter instruction: ");
            var duration = ParseDouble("duration in minutes");

            return recipeController.CreateRecipe(name, category, description, recipeIngredients, instruction, duration);
        }

        static double ParseDouble(string name)
        {
            while (true)
            {
                if (double.TryParse(Request($"Enter {name}: "), out double value))
                    return value;
                else
                    Console.Write($"Invalid input for {name}");
            }
        }

        static List<string> GetWords(string text)
        {
            List<string> words = new List<string>();

            string pattern = @"([\w]+(['`]\w+)?([ \w]+)?)";
            foreach (Match m in Regex.Matches(text, pattern))
            {
                words.Add(m.Value);
            }

            return words;
        }

        static void ShowItems<T>(List<T> models) where T : EntityBase
        {
            foreach (var item in models)
            {
                Console.WriteLine(typeof(T).Name + "\t" + item.Name);
            }
        }

        static string Request(string message)
        {
            Console.Write("\n" + message);
            var item = Console.ReadLine();
            Console.Clear();

            return item;
        }

        static void ShowRecipe(Recipe recipe)
        {
            Console.WriteLine("Name: " + recipe.Name);
            Console.WriteLine("Description: " + recipe.Description);

            Console.WriteLine("Ingredients: ");
            foreach (var item in recipe.RecipeIngredient)
            {
                Console.WriteLine("\t" + item.Ingredient.Name + " - " + item.Amount);
            }

            Console.WriteLine("Instruction: " + recipe.Instruction);
            Console.WriteLine("Duration in minutes: " + recipe.DurationInMinutes);
        }
    }
}
