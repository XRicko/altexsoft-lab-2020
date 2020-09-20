using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RecipeBook.SharedKernel.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity Get(string name);
        TEntity Get(TEntity entity);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void Remove(TEntity entity);
        void Remove(string name);
    }
}
