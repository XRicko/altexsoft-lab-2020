using RecipeBook.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RecipeBook.BL.Repository.Interfaces
{
    public interface IRepository<T> where T : ModelBase
    {
        T Get(string name);
        T Get(T model);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        void Add(T item);
    }
}
