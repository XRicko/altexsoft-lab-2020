﻿using RecipeBook.Core.Entities;
using RecipeBook.Core.Extensions;
using RecipeBook.SharedKernel.Interfaces;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public class IngredientController : ControllerBase
    {
        public IngredientController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<Ingredient> CreateIngredientAsync(string name)
        {
            string standardizedName = name.StandardizeName();
            var ingredient = await UnitOfWork.Repository.GetAsync<Ingredient>(standardizedName);

            if (ingredient == null)
                return new Ingredient(standardizedName);

            return ingredient;
        }
    }
}