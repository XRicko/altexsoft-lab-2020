﻿using Microsoft.EntityFrameworkCore;
using RecipeBook.SharedKernel;
using RecipeBook.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RecipeBook.Infrastructure.Data
{
    public class EfRepository : IRepository
    {
        private readonly RecipeBookContext context;

        public EfRepository(RecipeBookContext context)
        {
            this.context = context;
        }

        public Task<T> GetAsync<T>(string name) where T : BaseEntity
        {
            return context.Set<T>().SingleOrDefaultAsync(x => x.Name == name);
        }

        public Task<T> GetAsync<T>(T entity) where T : BaseEntity
        {
            return context.Set<T>().SingleOrDefaultAsync(x => x.Name == entity.Name);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : BaseEntity
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task AddAsync<T>(T entity) where T : BaseEntity
        {
            await context.Set<T>().AddAsync(entity);
        }

        public void Remove<T>(T entity) where T : BaseEntity
        {
            context.Set<T>().Remove(entity);
        }
    }
}
