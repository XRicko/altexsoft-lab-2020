using RecipeBook.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RecipeBook.BL.Repository.Interfaces
{
    interface IRepository<T> where T : ModelBase
    {
        T Get(ref string name);
        T Get(T model);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        void Add(T item);
    }
}
