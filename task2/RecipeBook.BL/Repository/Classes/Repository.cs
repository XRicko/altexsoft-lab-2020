using RecipeBook.BL.Controllers;
using RecipeBook.BL.Models;
using RecipeBook.BL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace RecipeBook.BL.Repository.Classes
{
    class Repository<T> : IRepository<T> where T : ModelBase
    {
        private readonly IDataManager manager = new JSONDataManager();
        private readonly List<T> data;

        public Repository()
        {
            data = manager.Load<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return data;
        }
        public T Get(T model)
        {
            var item = data.SingleOrDefault(x => x.Name == model.Name);
            return item;
        }
        public T Get(ref string name)
        {
            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            name = name.Trim();

            var tempName = name;
            var item = data.SingleOrDefault(x => x.Name == tempName);

            return item;
        }
        public void Add(T model)
        {
            var item = Get(model);

            if (item == null)
            {
                if (data.Count != 0)
                    model.Id = data.Last().Id + 1;

                data.Add(model);
            }
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            Func<T, bool> func = expression.Compile();
            Predicate<T> predicate = func.Invoke;

            return data.FindAll(predicate);
        }
    }
}
