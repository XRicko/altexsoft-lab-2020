﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RecipeBook.SharedKernel.Interfaces
{
    public interface IRepository
    {
        Task<T> GetAsync<T>(string name) where T : BaseEntity;
        Task<T> GetAsync<T>(T entity) where T : BaseEntity;
        Task<IEnumerable<T>> GetAllAsync<T>() where T : BaseEntity;
        Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : BaseEntity;

        Task AddAsync<T>(T entity) where T : BaseEntity;
        void Remove<T>(T entity) where T : BaseEntity;
    }
}
