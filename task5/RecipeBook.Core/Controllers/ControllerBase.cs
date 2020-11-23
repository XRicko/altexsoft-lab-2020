﻿using RecipeBook.SharedKernel;
using RecipeBook.SharedKernel.Extensions;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public abstract class ControllerBase
    {
        protected IUnitOfWork UnitOfWork { get; }

        public ControllerBase(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public Task<T> GetByNameAsync<T>(string name) where T : BaseEntity =>
            UnitOfWork.Repository.GetAsync<T>(name.StandardizeName());

        public Task<T> GetByIdAsync<T>(int id) where T : BaseEntity =>
            UnitOfWork.Repository.GetAsync<T>(id);

        public Task<IEnumerable<T>> GetItemsAsync<T>() where T : BaseEntity =>
            UnitOfWork.Repository.GetAllAsync<T>();

        public async Task AddItemAsync<T>(T entity) where T : BaseEntity
        {
            T item = await UnitOfWork.Repository.GetAsync(entity);

            if (item is null)
            {
                await UnitOfWork.Repository.AddAsync(entity);
                await UnitOfWork.SaveAsync();
            }
        }

        public async Task UpdateAsync<T>(T entity) where T : BaseEntity
        {
            UnitOfWork.Repository.Update(entity);
            await UnitOfWork.SaveAsync();
        }
    }
}
