﻿using RecipeBook.BL.Controllers;
using RecipeBook.BL.Models;
using RecipeBook.BL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RecipeBook.BL.Repository.Classes
{
    class Repository<T> : IRepository<T> where T : ModelBase
    {
        private readonly IDataManager manager;
        private readonly List<T> data;

        public Repository(IDataManager manager)
        {
            this.manager = manager;
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
        public T Get(string name)
        {
            var item = data.SingleOrDefault(x => x.Name == name);
            return item;
        }
        public void Add(T model)
        {
            if (data.Count != 0)
                model.Id = data.Last().Id + 1;

            data.Add(model);
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            Func<T, bool> func = expression.Compile();
            Predicate<T> predicate = func.Invoke;

            return data.FindAll(predicate);
        }
    }
}
