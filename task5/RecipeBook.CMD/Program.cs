using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RecipeBook.Core.Controllers;
using RecipeBook.Core.Entities;
using RecipeBook.Infrastructure.Extensions;
using RecipeBook.SharedKernel;
using RecipeBook.SharedKernel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.UI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Getting things ready...");

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
                        ShowItems(await ingredientController.GetItemsAsync<Ingredient>());

                        break;
                    case ConsoleKey.E:
                        logger.LogInformation("Getting records...");

                        var categories = await categoryController.GetTopCategoriesAsync();
                        ICollection<Recipe> recipies = new List<Recipe>();

                        while (true)
                        {
                            if (!categories.Any() && !recipies.Any())
                            {
                                Console.WriteLine("No elements here");
                                break;
                            }

                            ShowItems(categories);
                            ShowItems(recipies);

                            string name = Request("Enter name to go next or to see full recipe: ");

                            var rec = await recipeController.GetByNameAsync<Recipe>(name);
                            var category = await categoryController.GetByNameAsync<Category>(name);

                            if (!(rec is null))
                            {
                                ShowRecipe(rec);
                                break;
                            }
                            else if (category is null)
                                Console.WriteLine("Invalid input\n");
                            else
                            {
                                recipies = category.Recipes;
                                categories = category.Children;
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

        static async Task<Recipe> EnterRecipeAsync(RecipeController recipeController,
                                                   CategoryController categoryController,
                                                   IngredientController ingredientController)
        {
            string name = Request("Enter name of recipe: ");
            string categoryName = Request("Enter category (starting with subcats): ");

            Category category;

            while (true)
            {
                string key = Request("Is it a subcategory? (y/n): ");

                if (key == "y")
                {
                    string parentName = Request("Enter parent category: ");
                    var parentCategory = await categoryController.GetByNameAsync<Category>(parentName);
                    category = await categoryController.GetOrCreateCategoryAsync(categoryName, parentCategory.Id);

                    break;
                }
                if (key == "n")
                {
                    category = await categoryController.GetOrCreateCategoryAsync(categoryName);
                    break;
                }

                Console.Write("Invalid input");
            }

            string description = Request("Enter a brief description: ");
            string ingredients = Request("Enter ingredients (comma-separated): ");

            var recipeIngredients = new List<RecipeIngredient>();

            foreach (var item in ingredients.GetWords())
            {
                var recipeIngredient = new RecipeIngredient
                {
                    Ingredient = await ingredientController.GetOrCreateIngredientAsync(item)
                };

                recipeIngredients.Add(recipeIngredient);
            }

            foreach (var item in recipeIngredients)
            {
                string amount = Request($"Enter amount of {item.Ingredient.Name} (e.g., 1 tbsp, 200 grams): ");
                item.Amount = amount;
            }

            string instruction = Request("Enter instruction: ");
            double duration = ParseDouble("duration in minutes");

            return await recipeController.GetOrCreateRecipeAsync(name, category, description, recipeIngredients, instruction, duration);
        }

        static double ParseDouble(string name)
        {
            while (true)
            {
                if (!double.TryParse(Request($"Enter {name}: "), out double value))
                    Console.Write($"Invalid input for {name}");

                return value;
            }
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
            foreach (var item in recipe.RecipeIngredients)
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
