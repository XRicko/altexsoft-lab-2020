using Microsoft.EntityFrameworkCore;
using RecipeBook.SharedKernel;
using RecipeBook.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RecipeBook.Core.Repository.Classes
{
    class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbContext context;

        public Repository(DbContext context)
        {
            this.context = context;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }

        public TEntity Get(TEntity entity)
        {
            return context.Set<TEntity>().SingleOrDefault(x => x.Name == entity.Name);
        }

        public TEntity Get(string name)
        {
            return context.Set<TEntity>().SingleOrDefault(x => x.Name == name);
        }

        public void Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Where(predicate).ToList();
        }

        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public void Remove(string name)
        {
            context.Set<TEntity>().Remove(context.Set<TEntity>().SingleOrDefault(e => e.Name == name));
        }
    }
}
