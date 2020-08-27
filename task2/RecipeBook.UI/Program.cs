using Newtonsoft.Json;
using RecipeBook.BL.Controllers;
using RecipeBook.BL.Models;
using RecipeBook.BL.Repository.Classes;
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
            var serializer = new JsonSerializer();
            var manager = new JSONDataManager(serializer);
            var unitOfWork = new UnitOfWork(manager);

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

                            var name = Dialog("Enter name to go next or to see full recipe: ");
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
            var name = Dialog("Enter name of recipe: ");
            var categoryName = Dialog("Enter category (starting with subcats): ");

            Category category;

            while (true)
            {
                var key = Dialog("Is it a subcategory? (y/n): ");

                if (key == "y")
                {
                    var parentName = Dialog("Enter parent category: ");
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

            var description = Dialog("Enter a brief description: ");
            var text = Dialog("Enter ingredients (comma-separated): ");

            var recipeIngredients = new List<RecipeIngredient>();

            foreach (var item in GetWords(text))
            {
                var recipeIngredient = new RecipeIngredient
                {
                    Ingredient = ingredientController.CreateIngredient(item)
                };

                recipeIngredients.Add(recipeIngredient);
            }

            foreach (var item in recipeIngredients)
            {
                var amount = Dialog($"Enter amount of {item.Ingredient.Name} (e.g., 1 tablespoon, 200 grams): ");
                item.Amount = amount;
            }

            int steps = ParseInt("amount of steps to cook this dish");
            var instruction = new string[steps];

            for (int i = 0; i < instruction.Length; i++)
            {
                var step = Dialog($"Enter step {i + 1}: ");
                instruction[i] = step;
            }

            var duration = ParseDouble("duration in minutes");

            return recipeController.CreateRecipe(name, category, description, recipeIngredients, instruction, duration);

        }
        static double ParseDouble(string name)
        {
            while (true)
            {
                if (double.TryParse(Dialog($"Enter {name}: "), out double value))
                    return value;
                else
                    Console.Write($"Invalid input for {name}");
            }
        }
        static int ParseInt(string name)
        {
            while (true)
            {
                if (int.TryParse(Dialog($"Enter {name}: "), out int value))
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
        static void ShowItems<T>(List<T> models) where T : ModelBase
        {
            foreach (var item in models)
            {
                Console.WriteLine(typeof(T).Name + "\t" + item.Name);
            }
        }
        static string Dialog(string message)
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
            foreach (var item in recipe.Ingredients)
            {
                Console.WriteLine("\t" + item.Ingredient.Name + " - " + item.Amount);
            }

            Console.WriteLine("Steps: ");
            foreach (var item in recipe.Instruction)
            {
                Console.WriteLine("\t" + item);
            }

            Console.WriteLine("Duration in minutes: " + recipe.DurationInMinutes);
        }
    }
}
