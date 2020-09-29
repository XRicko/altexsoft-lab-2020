using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.Infrastructure.Extensions;
using RecipeBook.SharedKernel;
using RecipeBook.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecipeBook.UI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Getting repository...");
            var unitOfWork = host.Services.GetRequiredService<IUnitOfWork>();

            var categoryController = host.Services.GetRequiredService<CategoryController>();
            var ingredientController = host.Services.GetRequiredService<IngredientController>();
            var recipeController = host.Services.GetRequiredService<RecipeController>();

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
                        var recipe = await EnterRecipeAsync(recipeController, categoryController, ingredientController);
                        await recipeController.AddRecipeAsync(recipe);

                        break;
                    case ConsoleKey.F:
                        logger.LogInformation("Getting ingredients...");

                        foreach (var item in await ingredientController.GetItemsAsync<Ingredient>())
                        {
                            Console.WriteLine(item.Name);
                        }

                        break;
                    case ConsoleKey.E:
                        logger.LogInformation("Getting records...");

                        var categories = await categoryController.GetCategoriesAsync(null);
                        var recipies = await recipeController.GetRecipesInCategoryAsync(null);

                        while (true)
                        {
                            if (!categories.Any() && !recipies.Any())
                            {
                                Console.WriteLine("No elements here");
                                break;
                            }

                            ShowItems(categories);
                            ShowItems(recipies);

                            var name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Request("Enter name to go next or to see full recipe: ")
                                .ToLower())
                                .Trim();

                            var rec = recipies.SingleOrDefault(r => r.Name == name);
                            var category = categories.SingleOrDefault(c => c.Name == name);

                            if (rec != null)
                            {
                                ShowRecipe(rec);
                                break;
                            }
                            else if (category == null)
                                Console.WriteLine("Invalid input\n");
                            else
                            {
                                recipies = await recipeController.GetRecipesInCategoryAsync(name);
                                categories = await categoryController.GetCategoriesAsync(category.Id);
                            }
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

        static async Task<Recipe> EnterRecipeAsync(RecipeController recipeController, CategoryController categoryController, IngredientController ingredientController)
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
                    category = await categoryController.CreateCategoryAsync(categoryName, parentName);

                    break;
                }
                if (key == "n")
                {
                    category = await categoryController.CreateCategoryAsync(categoryName);
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
                    Ingredient = await ingredientController.CreateIngredientAsync(item)
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

            return await recipeController.CreateRecipeAsync(name, category, description, recipeIngredients, instruction, duration);
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

        static void ShowItems<T>(IEnumerable<T> models) where T : BaseEntity
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddInfrastructure(context.Configuration.GetConnectionString("DefaultConnection"));

                services.AddTransient<IngredientController>();
                services.AddTransient<CategoryController>();
                services.AddTransient<RecipeController>();
            })
            .ConfigureLogging(config =>
            {
                config.ClearProviders();
                config.AddConsole();
            });
    }
}
