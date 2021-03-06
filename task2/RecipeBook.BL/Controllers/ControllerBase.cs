﻿using RecipeBook.BL.Repository.Interfaces;
using System.Globalization;

namespace RecipeBook.BL.Controllers
{
    public abstract class ControllerBase
    {
        protected readonly IUnitOfWork unitOfWork;

        public ControllerBase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        protected string StandardizeName(string name)
        {
            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            name = name.Trim();

            return name;
        }
    }
}
