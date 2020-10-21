using RecipeBook.Core.Extensions;
using RecipeBook.SharedKernel;
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

        public Task<T> GetByNameAsync<T>(string name) where T : BaseEntity
        {
            return UnitOfWork.Repository.GetAsync<T>(name.StandardizeName());
        }

        public Task<IEnumerable<T>> GetItemsAsync<T>() where T : BaseEntity
        {
            return UnitOfWork.Repository.GetAllAsync<T>();
        }

        public async Task AddItemAsync<T>(T entity) where T : BaseEntity
        {
            T item = await UnitOfWork.Repository.GetAsync(entity);

            if (item == null)
            {
                var items = await GetItemsAsync<T>();

                if (items != null && items.Any())
                    entity.Id = items.OrderBy(i => i.Id).Last().Id + 1;

                await UnitOfWork.Repository.AddAsync(entity);
                await UnitOfWork.SaveAsync();
            }
        }
    }
}
